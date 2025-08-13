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
}