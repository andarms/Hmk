using System.Collections;
using System.Globalization;
using System.Numerics;
using System.Xml.Linq;
using Hmk.Engine.Core;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Resources;

public abstract class Resource : ISerializable
{
  public virtual void Deserialize(XElement element)
  {
    ArgumentNullException.ThrowIfNull(element);

    // If this element is a reference to another Resource, copy values from referenced instance
    var refKey = element.Attribute("Ref")?.Value ?? element.Attribute("Path")?.Value;
    if (!string.IsNullOrWhiteSpace(refKey) && ResourcesManager.Resources.TryGetValue(refKey!, out var referenced))
    {
      // Copy [Save]-annotated properties from the referenced instance to this instance
      var refProps = referenced.GetType().GetProperties()
        .Where(p => Attribute.IsDefined(p, typeof(SaveAttribute)) && p.CanRead);
      foreach (var prop in refProps)
      {
        try
        {
          var targetProp = GetType().GetProperty(prop.Name);
          if (targetProp != null && targetProp.CanWrite && targetProp.PropertyType.IsAssignableFrom(prop.PropertyType))
          {
            targetProp.SetValue(this, prop.GetValue(referenced));
          }
        }
        catch { }
      }
      // Continue to allow local overrides below
    }

    var props = GetType().GetProperties()
      .Where(p => Attribute.IsDefined(p, typeof(SaveAttribute)) && p.CanWrite);

    foreach (var prop in props)
    {
      var el = element.Element(prop.Name)
               ?? element.Elements().FirstOrDefault(e => string.Equals(e.Attribute("Property")?.Value, prop.Name, StringComparison.Ordinal));
      if (el == null) continue;

      var value = DeserializeValueForType(prop.PropertyType, el);
      if (value != null || prop.PropertyType.IsClass)
      {
        try { prop.SetValue(this, value); } catch { }
      }
    }
  }

  public virtual XElement Serialize()
  {
    XElement element = new("Resource");
    element.SetAttributeValue("Type", GetType().FullName);

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

  private static object? DeserializeValueForType(Type targetType, XElement element)
  {
    // Support lightweight <ResourceReference Path="key" /> or legacy <ResourceRef>key</ResourceRef>
    if (typeof(Resource).IsAssignableFrom(targetType))
    {
      if (string.Equals(element.Name.LocalName, "ResourceReference", StringComparison.Ordinal) || string.Equals(element.Name.LocalName, "ResourceRef", StringComparison.Ordinal))
      {
        var key = element.Attribute("Path")?.Value ?? element.Attribute("Ref")?.Value ?? element.Value;
        if (!string.IsNullOrWhiteSpace(key) && ResourcesManager.Resources.TryGetValue(key!, out var found))
        {
          return found;
        }
      }
      // Also allow any element carrying Ref/Path to mean reference
      var refKey = element.Attribute("Ref")?.Value ?? element.Attribute("Path")?.Value;
      if (!string.IsNullOrWhiteSpace(refKey) && ResourcesManager.Resources.TryGetValue(refKey!, out var referenced))
      {
        return referenced;
      }
    }

    // Handle explicit type hint on the element for polymorphic/interface targets
    // e.g., a property typed as an interface where the XML contains Type="ConcreteType"
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
        // Fallback: try to convert value to the resolved simple type if applicable
        try
        {
          return Convert.ChangeType(element.Value, resolved, CultureInfo.InvariantCulture);
        }
        catch
        {
          // ignore and continue to the default handling below
        }
      }
    }

    // Generic List<T>
    if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>))
    {
      var itemType = targetType.GetGenericArguments()[0];
      var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType))!;

      foreach (var child in element.Elements())
      {
        // If the child is a serialized GO/Resource, pass it through; otherwise use item-type aware reading
        var value = DeserializeListItem(itemType, child);
        if (value != null || itemType.IsClass)
        {
          list.Add(value!);
        }
      }
      return list;
    }

    if (targetType == typeof(string))
    {
      return element.Value;
    }
    if (targetType == typeof(int))
    {
      return int.TryParse(element.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i) ? i : 0;
    }
    if (targetType == typeof(float))
    {
      return float.TryParse(element.Value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var f) ? f : 0f;
    }
    if (targetType == typeof(Vector2))
    {
      return element.ToVector2();
    }
    if (targetType == typeof(Rectangle))
    {
      return element.ToRectangle();
    }
    if (targetType == typeof(Color))
    {
      return element.ToColor();
    }

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

    try
    {
      return Convert.ChangeType(element.Value, targetType, CultureInfo.InvariantCulture);
    }
    catch
    {
      return null;
    }
  }

  private static object? DeserializeListItem(Type itemType, XElement element)
  {
    // Allow explicit Type attribute to guide polymorphic items
    var explicitTypeName = element.Attribute("Type")?.Value;
    if (!string.IsNullOrWhiteSpace(explicitTypeName))
    {
      var resolved = ResolveType(explicitTypeName);
      if (resolved != null && itemType.IsAssignableFrom(resolved))
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
        try { return Convert.ChangeType(element.Value, resolved, CultureInfo.InvariantCulture); } catch { }
      }
    }

    if (itemType == typeof(string)) return element.Value;
    if (itemType == typeof(int)) return int.TryParse(element.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i) ? i : 0;
    if (itemType == typeof(float)) return float.TryParse(element.Value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var f) ? f : 0f;
    if (itemType == typeof(Vector2)) return element.ToVector2();
    if (itemType == typeof(Rectangle)) return element.ToRectangle();
    if (itemType == typeof(Color)) return element.ToColor();

    if (typeof(Resource).IsAssignableFrom(itemType))
    {
      var typeAttr = element.Attribute("Type")?.Value;
      var t = ResolveType(typeAttr, typeof(Resource)) ?? itemType;
      var res = Activator.CreateInstance(t) as Resource;
      res?.Deserialize(element);
      return res;
    }
    if (typeof(GameObject).IsAssignableFrom(itemType))
    {
      var typeAttr = element.Attribute("Type")?.Value;
      var t = ResolveType(typeAttr, typeof(GameObject)) ?? itemType;
      var go = Activator.CreateInstance(t) as GameObject;
      go?.Deserialize(element);
      return go;
    }

    try { return Convert.ChangeType(element.Value, itemType, CultureInfo.InvariantCulture); } catch { return null; }
  }

  private static Type? ResolveType(string? nameOrFullName, Type? mustInherit = null)
  {
    if (string.IsNullOrWhiteSpace(nameOrFullName)) return null;
    foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
    {
      var t = asm.GetType(nameOrFullName, throwOnError: false, ignoreCase: false) ?? asm.GetTypes().FirstOrDefault(tp => tp.Name == nameOrFullName);
      if (t != null && (mustInherit == null || mustInherit.IsAssignableFrom(t)))
      {
        return t;
      }
    }
    return null;
  }

  public T Preload<T>(string filePath) where T : GameObject, new()
  {
    var newObject = GameObjectSerializerExtensions.LoadFromXml(filePath);
    return newObject as T ?? throw new InvalidOperationException($"Resource at '{filePath}' is not a {typeof(T).Name}.");
  }
}