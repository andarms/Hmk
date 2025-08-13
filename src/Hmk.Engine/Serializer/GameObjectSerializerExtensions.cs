using System.Globalization;
using System.Numerics;
using System.Xml.Linq;
using Hmk.Engine.Collision;
using Hmk.Engine.Core;
using Hmk.Engine.Resources;

namespace Hmk.Engine.Serializer;

public static class GameObjectSerializerExtensions
{
  public static GameObject LoadFromXElement(XElement element)
  {
    ArgumentNullException.ThrowIfNull(element);
    var instance = CreateGameObjectFromElement(element) ?? new GameObject();
    instance.Deserialize(element);
    return instance;
  }

  public static GameObject LoadFromXml(string filePath)
  {
    ArgumentNullException.ThrowIfNull(filePath);
    var doc = XDocument.Load(filePath);
    var root = doc.Root ?? throw new InvalidOperationException("XML has no root element");
    return LoadFromXElement(root);
  }

  public static XElement Serialize(this GameObject gameObject)
  {
    XElement element = new("GameObject");
    element.SetAttributeValue("Type", gameObject.GetType().FullName);
    if (!string.IsNullOrEmpty(gameObject.Name))
    {
      element.SetAttributeValue("Name", gameObject.Name);
    }
    element.Add(gameObject.Position.Serialize("Position"));

    if (gameObject.Collider != null)
    {
      element.Add(gameObject.Collider.Serialize("Collider"));
    }

    var members = gameObject.GetType().GetProperties()
        .Where(prop => Attribute.IsDefined(prop, typeof(SaveAttribute)));


    if (members.Any())
    {
      foreach (var member in members)
      {
        XElement memberElement = MemberSerializer.Serialize(member, member.GetValue(gameObject));
        element.Add(memberElement);
      }
    }

    if (gameObject.Children.Count > 0)
    {
      XElement childrenElement = new("Children");
      element.Add(childrenElement);
      foreach (var child in gameObject.Children)
      {
        childrenElement.Add(child.Serialize());
      }
    }

    if (gameObject.Traits.Count > 0)
    {
      XElement traitsElement = new("Traits");
      element.Add(traitsElement);
      foreach (var trait in gameObject.Traits.Values)
      {
        traitsElement.Add(trait.Serialize());
      }
    }

    return element;
  }

  public static void Deserialize(this GameObject gameObject, XElement element)
  {
    ArgumentNullException.ThrowIfNull(element);

    // Basic attributes
    var nameAttr = element.Attribute("Name")?.Value;
    if (!string.IsNullOrWhiteSpace(nameAttr))
    {
      gameObject.Name = nameAttr!;
    }

    var positionElement = element.Element("Position");
    if (positionElement != null)
    {
      gameObject.Position = positionElement.ToVector2();
    }


    var colliderElement = element.Element("Collider");
    if (colliderElement != null)
    {
      gameObject.SetCollider(colliderElement.ToCollider());
    }


    // [Save] properties on this GameObject
    DeserializeSavedProperties(gameObject, element);

    var childrenElement = element.Element("Children");
    if (childrenElement != null)
    {
      foreach (var childEl in childrenElement.Elements())
      {
        var child = CreateGameObjectFromElement(childEl);
        if (child != null)
        {
          child.Deserialize(childEl);
          gameObject.AddChild(child);
        }
      }
    }

    // Traits
    var traitsElement = element.Element("Traits");
    if (traitsElement != null)
    {
      foreach (var traitEl in traitsElement.Elements())
      {
        var typeName = traitEl.Attribute("Type")?.Value;
        if (string.IsNullOrWhiteSpace(typeName)) continue;

        var traitType = ResolveType(typeName, typeof(Trait));
        if (traitType == null) continue;

        if (Activator.CreateInstance(traitType) is Trait trait)
        {
          trait.Deserialize(traitEl);
          gameObject.AddTrait(trait);
        }
      }
    }
  }


  public static XElement Serialize(this Vector2 data, string tagName = "Vector2")
  {
    var element = new XElement(tagName);
    element.SetAttributeValue("X", data.X.ToString(CultureInfo.InvariantCulture));
    element.SetAttributeValue("Y", data.Y.ToString(CultureInfo.InvariantCulture));
    return element;
  }

  // Prefer returning a value for value types instead of attempting in-place mutation.
  public static Vector2 ToVector2(this XElement element)
  {
    ArgumentNullException.ThrowIfNull(element);
    var x = float.Parse(element.Attribute("X")?.Value ?? "0", CultureInfo.InvariantCulture);
    var y = float.Parse(element.Attribute("Y")?.Value ?? "0", CultureInfo.InvariantCulture);
    return new Vector2(x, y);
  }

  public static XElement Serialize(this Collider data, string tagName = "Collider")
  {
    var element = new XElement(tagName);
    element.Add(data.Size.Serialize("Size"));
    element.Add(data.Offset.Serialize("Offset"));
    return element;
  }

  public static Collider ToCollider(this XElement element)
  {
    ArgumentNullException.ThrowIfNull(element);
    XElement? sizeElement = element.Element("Size");
    XElement? offsetElement = element.Element("Offset");
    ArgumentNullException.ThrowIfNull(sizeElement, "Size element is required for Collider deserialization.");
    ArgumentNullException.ThrowIfNull(offsetElement, "Offset element is required for Collider deserialization.");

    return new Collider
    {
      Size = sizeElement.ToVector2(),
      Offset = offsetElement.ToVector2()
    };
  }

  public static XElement Serialize(this Color data, string tagName = "Color")
  {
    var element = new XElement(tagName);
    element.SetAttributeValue("R", data.R);
    element.SetAttributeValue("G", data.G);
    element.SetAttributeValue("B", data.B);
    element.SetAttributeValue("A", data.A);
    return element;
  }

  public static Color ToColor(this XElement element)
  {
    ArgumentNullException.ThrowIfNull(element);
    var r = byte.Parse(element.Attribute("R")?.Value ?? "0", CultureInfo.InvariantCulture);
    var g = byte.Parse(element.Attribute("G")?.Value ?? "0", CultureInfo.InvariantCulture);
    var b = byte.Parse(element.Attribute("B")?.Value ?? "0", CultureInfo.InvariantCulture);
    var a = byte.Parse(element.Attribute("A")?.Value ?? "0", CultureInfo.InvariantCulture);
    return new Color(r, g, b, a);
  }

  public static XElement Serialize(this Rectangle data, string tagName = "Rectangle")
  {
    var element = new XElement(tagName);
    element.SetAttributeValue("X", data.X.ToString(CultureInfo.InvariantCulture));
    element.SetAttributeValue("Y", data.Y.ToString(CultureInfo.InvariantCulture));
    element.SetAttributeValue("Width", data.Width.ToString(CultureInfo.InvariantCulture));
    element.SetAttributeValue("Height", data.Height.ToString(CultureInfo.InvariantCulture));
    return element;
  }

  public static Rectangle ToRectangle(this XElement element)
  {
    ArgumentNullException.ThrowIfNull(element);
    var x = float.Parse(element.Attribute("X")?.Value ?? "0", CultureInfo.InvariantCulture);
    var y = float.Parse(element.Attribute("Y")?.Value ?? "0", CultureInfo.InvariantCulture);
    var w = float.Parse(element.Attribute("Width")?.Value ?? "0", CultureInfo.InvariantCulture);
    var h = float.Parse(element.Attribute("Height")?.Value ?? "0", CultureInfo.InvariantCulture);
    return new Rectangle(x, y, w, h);
  }

  private static GameObject? CreateGameObjectFromElement(XElement element)
  {
    var typeAttr = element.Attribute("Type")?.Value;
    if (string.IsNullOrWhiteSpace(typeAttr)) return new GameObject();

    var t = ResolveType(typeAttr, typeof(GameObject));
    if (t == null) return new GameObject();
    return Activator.CreateInstance(t) as GameObject;
  }

  private static Resource? CreateResourceFromElement(XElement element)
  {
    var typeAttr = element.Attribute("Type")?.Value;
    if (string.IsNullOrWhiteSpace(typeAttr)) return null;

    var t = ResolveType(typeAttr, typeof(Resource));
    if (t == null) return null;
    return Activator.CreateInstance(t) as Resource;
  }

  private static void DeserializeSavedProperties(GameObject gameObject, XElement parent)
  {
    var members = gameObject.GetType().GetProperties()
      .Where(prop => Attribute.IsDefined(prop, typeof(SaveAttribute)) && prop.CanWrite);

    foreach (var member in members)
    {
      // Two possible encodings:
      // 1) Primitive/struct types: element name equals property name
      // 2) Complex types (GameObject/Resource): element with attribute Property=property name
      var el = parent.Element(member.Name)
               ?? parent.Elements().FirstOrDefault(e => string.Equals(e.Attribute("Property")?.Value, member.Name, StringComparison.Ordinal));

      if (el == null) continue;

      var targetType = member.PropertyType;
      object? value = DeserializeValueForType(targetType, el);
      if (value != null || targetType.IsClass)
      {
        try { member.SetValue(gameObject, value); }
        catch { /* ignore assignment failures */ }
      }
    }
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
    if (typeof(GameObject).IsAssignableFrom(targetType))
    {
      // Expecting <GameObject Type="..." Property="...">...
      var child = CreateGameObjectFromElement(element) ?? Activator.CreateInstance(targetType) as GameObject;
      if (child != null)
      {
        child.Deserialize(element);
      }
      return child;
    }
    if (typeof(Resource).IsAssignableFrom(targetType))
    {
      // Expecting <Resource Type="..." Property="...">...
      var res = CreateResourceFromElement(element) ?? Activator.CreateInstance(targetType) as Resource;
      if (res != null)
      {
        res.Deserialize(element);
      }
      return res;
    }

    // Fallback: try ChangeType from string
    try
    {
      return Convert.ChangeType(element.Value, targetType, CultureInfo.InvariantCulture);
    }
    catch
    {
      return null;
    }
  }

  private static Type? ResolveType(string nameOrFullName, Type? mustInherit = null)
  {
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
