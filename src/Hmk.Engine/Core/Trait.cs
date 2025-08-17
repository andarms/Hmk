using System.Globalization;
using System.Numerics;
using System.Xml.Linq;
using Hmk.Engine.Serializer;
using Hmk.Engine.Resources;

namespace Hmk.Engine.Core;

public abstract class Trait : ISerializable
{
  public string Type => GetType().Name;

  public virtual XElement Serialize()
  {
    XElement element = new("Trait");
    element.SetAttributeValue("Type", Type);

    // Serialize [Save] properties on this Trait
    var props = GetType().GetProperties()
      .Where(p => Attribute.IsDefined(p, typeof(SaveAttribute)));

    foreach (var prop in props)
    {
      var value = prop.GetValue(this);
      XElement memberElement = MemberSerializer.Serialize(prop, value);
      element.Add(memberElement);
    }

    return element;
  }

  public virtual void Deserialize(XElement element)
  {
    ArgumentNullException.ThrowIfNull(element);

    // Deserialize [Save] properties on this Trait
    var props = GetType().GetProperties()
      .Where(p => Attribute.IsDefined(p, typeof(SaveAttribute)));

    foreach (var prop in props)
    {
      // Try either <PropertyName>...</PropertyName> or any element with Property="PropertyName"
      var el = element.Element(prop.Name)
               ?? element.Elements().FirstOrDefault(e => string.Equals(e.Attribute("Property")?.Value, prop.Name, StringComparison.Ordinal));
      if (el == null) continue;

      if (prop.CanWrite)
      {
        var value = DeserializeValueForType(prop.PropertyType, el);
        if (value != null || prop.PropertyType.IsClass)
        {
          try { prop.SetValue(this, value); } catch { }
        }
      }
      else
      {
        // Read-only property: if it's a Resource/GameObject/ISerializable instance, try in-place deserialize
        var current = prop.GetValue(this);
        switch (current)
        {
          case Resource res:
            res.Deserialize(el);
            break;
          case GameObject go:
            go.Deserialize(el);
            break;
          case ISerializable serializable:
            serializable.Deserialize(el);
            break;
        }
      }
    }
  }

  private static object? DeserializeValueForType(Type targetType, XElement element)
  {
    // Handle explicit type hint on the element for polymorphic/interface targets
    var explicitTypeName = element.Attribute("Type")?.Value;
    if (!string.IsNullOrWhiteSpace(explicitTypeName))
    {
      var resolved = ResolveType(explicitTypeName);
      if (resolved != null && targetType.IsAssignableFrom(resolved))
      {
        if (typeof(Resource).IsAssignableFrom(resolved))
        {
          var res = Activator.CreateInstance(resolved) as Resource;
          res?.Deserialize(element);
          return res;
        }
        if (typeof(GameObject).IsAssignableFrom(resolved))
        {
          var go = Activator.CreateInstance(resolved) as GameObject;
          go?.Deserialize(element);
          return go;
        }
        // Fallback for simple types
        try { return Convert.ChangeType(element.Value, resolved, CultureInfo.InvariantCulture); } catch { }
      }
    }

    // Handle List<T>
    if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>))
    {
      var itemType = targetType.GetGenericArguments()[0];
      var list = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType))!;
      foreach (var child in element.Elements())
      {
        var value = DeserializeListItem(itemType, child);
        if (value != null || itemType.IsClass)
        {
          list.Add(value!);
        }
      }
      return list;
    }

    if (targetType == typeof(string))
      return element.Value;
    if (targetType == typeof(int))
      return int.TryParse(element.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i) ? i : 0;
    if (targetType == typeof(float))
      return float.TryParse(element.Value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var f) ? f : 0f;
    if (targetType == typeof(Vector2))
      return element.ToVector2();
    if (targetType == typeof(Rectangle))
      return element.ToRectangle();
    if (targetType == typeof(Color))
      return element.ToColor();

    if (typeof(Resource).IsAssignableFrom(targetType))
    {
      var typeAttr = element.Attribute("Type")?.Value;
      var t = ResolveType(typeAttr, typeof(Resource)) ?? targetType;
      var res = Activator.CreateInstance(t) as Resource;
      res?.Deserialize(element);
      return res;
    }
    if (typeof(GameObject).IsAssignableFrom(targetType))
    {
      var typeAttr = element.Attribute("Type")?.Value;
      var t = ResolveType(typeAttr, typeof(GameObject)) ?? targetType;
      var go = Activator.CreateInstance(t) as GameObject;
      go?.Deserialize(element);
      return go;
    }

    try { return Convert.ChangeType(element.Value, targetType, CultureInfo.InvariantCulture); } catch { return null; }
  }

  private static object? DeserializeListItem(Type itemType, XElement element)
  {
    var explicitTypeName = element.Attribute("Type")?.Value;
    if (!string.IsNullOrWhiteSpace(explicitTypeName))
    {
      var resolved = ResolveType(explicitTypeName);
      if (resolved != null && itemType.IsAssignableFrom(resolved))
      {
        if (typeof(Resources.Resource).IsAssignableFrom(resolved))
        {
          var res = Activator.CreateInstance(resolved) as Resources.Resource;
          res?.Deserialize(element);
          return res;
        }
        if (typeof(GameObject).IsAssignableFrom(resolved))
        {
          var go = Activator.CreateInstance(resolved) as GameObject;
          go?.Deserialize(element);
          return go;
        }
        try { return Convert.ChangeType(element.Value, resolved, CultureInfo.InvariantCulture); } catch { }
      }
    }

    if (itemType == typeof(string)) return element.Value;
    if (itemType == typeof(int)) return int.TryParse(element.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i) ? i : 0;
    if (itemType == typeof(float)) return float.TryParse(element.Value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var f) ? f : 0f;
    if (itemType == typeof(Vector2)) return element.ToVector2();
    if (itemType == typeof(Rectangle)) return element.ToRectangle();
    if (itemType == typeof(Color)) return element.ToColor();

    if (typeof(GameObject).IsAssignableFrom(itemType))
    {
      var typeAttr = element.Attribute("Type")?.Value;
      var t = ResolveType(typeAttr ?? itemType.FullName!, typeof(GameObject)) ?? itemType;
      var go = Activator.CreateInstance(t) as GameObject;
      go?.Deserialize(element);
      return go;
    }
    if (typeof(Resources.Resource).IsAssignableFrom(itemType))
    {
      var typeAttr = element.Attribute("Type")?.Value;
      var t = ResolveType(typeAttr ?? itemType.FullName!, typeof(Resources.Resource)) ?? itemType;
      var res = Activator.CreateInstance(t) as Resources.Resource;
      res?.Deserialize(element);
      return res;
    }

    try { return Convert.ChangeType(element.Value, itemType, CultureInfo.InvariantCulture); } catch { return null; }
  }

  private static Type? ResolveType(string? nameOrFullName, Type? mustInherit = null)
  {
    if (string.IsNullOrWhiteSpace(nameOrFullName)) return null;
    foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
    {
      var t = asm.GetType(nameOrFullName, throwOnError: false, ignoreCase: false)
              ?? asm.GetTypes().FirstOrDefault(tp => tp.Name == nameOrFullName);
      if (t != null && (mustInherit == null || mustInherit.IsAssignableFrom(t)))
      {
        return t;
      }
    }
    return null;
  }
}
