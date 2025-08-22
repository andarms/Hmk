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
  .Where(prop => Attribute.IsDefined(prop, typeof(SaveAttribute)));

    foreach (var member in members)
    {
      // Two possible encodings:
      // 1) Primitive/struct types: element name equals property name
      // 2) Complex types (GameObject/Resource): element with attribute Property=property name
      var el = parent.Element(member.Name)
               ?? parent.Elements().FirstOrDefault(e => string.Equals(e.Attribute("Property")?.Value, member.Name, StringComparison.Ordinal));

      if (el == null) continue;

      var targetType = member.PropertyType;

      // If property is writable, use the generic value deserializer
      if (member.CanWrite)
      {
        object? value = DeserializeValueForType(targetType, el);
        if (value != null || targetType.IsClass)
        {
          try { member.SetValue(gameObject, value); }
          catch { /* ignore assignment failures */ }
        }
        continue;
      }

      // Read-only [Save] property: support in-place population for common container types
      var current = member.GetValue(gameObject);
      if (current == null) continue;

      // Handle Dictionary<K,V> patterns, e.g., AnimationController.Animations
      var curType = current.GetType();
      if (curType.IsGenericType && curType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
      {
        var args = curType.GetGenericArguments();
        var keyType = args[0];
        var valType = args[1];

        // Only support string keys for now
        if (keyType == typeof(string))
        {
          var dict = (System.Collections.IDictionary)current;
          foreach (var itemEl in el.Elements("Item"))
          {
            var key = itemEl.Attribute("Key")?.Value ?? string.Empty;

            object? valueObj = null;
            // Prefer first child element as complex value; else use Value attribute or inner text
            var child = itemEl.Elements().FirstOrDefault();
            if (child != null)
            {
              valueObj = DeserializeValueForType(valType, child);
            }
            else
            {
              var valueAttr = itemEl.Attribute("Value")?.Value;
              var text = valueAttr ?? itemEl.Value;
              valueObj = ConvertSimple(valType, text);
            }

            if (valueObj != null || valType.IsClass)
            {
              dict[key] = valueObj!;
            }
          }
        }
        continue;
      }

      // If it's a GameObject/Resource/ISerializable instance, try in-place deserialize
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

  private static object? ConvertSimple(Type t, string? text)
  {
    text = text?.Trim() ?? string.Empty;
    if (t == typeof(string)) return text;
    if (t == typeof(int)) return int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i) ? i : 0;
    if (t == typeof(float)) return float.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var f) ? f : 0f;
    if (t.IsEnum)
    {
      try { return Enum.Parse(t, text, ignoreCase: true); } catch { return null; }
    }
    try { return Convert.ChangeType(text, t, CultureInfo.InvariantCulture); } catch { return null; }
  }

  private static object? DeserializeValueForType(Type targetType, XElement element)
  {
    // Resource reference via <ResourceReference Path="..."/> (preferred) or legacy <ResourceRef/>, or Ref/Path attribute on element
    var isRefElement = string.Equals(element.Name.LocalName, "ResourceReference", StringComparison.Ordinal)
               || string.Equals(element.Name.LocalName, "ResourceRef", StringComparison.Ordinal)
                         || element.Attribute("Ref") != null
                         || element.Attribute("Path") != null;
    if (isRefElement)
    {
      var key = element.Attribute("Path")?.Value ?? element.Attribute("Ref")?.Value ?? element.Value;
      key = key?.Trim();
      if (!string.IsNullOrWhiteSpace(key))
      {
        if (ResourcesManager.Resources.TryGetValue(key!, out var foundRes))
        {
          // If the target type is a ResourceReference<T>, construct one and set Path.
          if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Hmk.Engine.Resources.ResourceReference<>))
          {
            try
            {
              var wrapper = Activator.CreateInstance(targetType);
              var pathProp = targetType.GetProperty("Path");
              pathProp?.SetValue(wrapper, key);
              return wrapper;
            }
            catch { /* fallthrough */ }
          }

          // If the referenced resource instance can be assigned to the target type (including interfaces like ISprite), return it directly.
          if (targetType.IsInstanceOfType(foundRes))
          {
            return foundRes;
          }

          // If the target expects a Resource base, also return
          if (typeof(Resources.Resource).IsAssignableFrom(targetType))
          {
            return foundRes;
          }
        }
      }
    }

    // Polymorphic/interface handling via explicit Type attribute on the element
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
        try
        {
          return Convert.ChangeType(element.Value, resolved, CultureInfo.InvariantCulture);
        }
        catch
        {
          // ignore and continue to default handling below
        }
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
    if (targetType.IsEnum)
    {
      try { return Enum.Parse(targetType, element.Value.Trim(), ignoreCase: true); } catch { return null; }
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
        if (typeof(Core.GameObject).IsAssignableFrom(resolved))
        {
          var go = Activator.CreateInstance(resolved) as Core.GameObject;
          go?.Deserialize(element);
          return go;
        }
        try { return Convert.ChangeType(element.Value, resolved, CultureInfo.InvariantCulture); } catch { }
      }
    }

    if (itemType == typeof(string)) return element.Value;
    if (itemType == typeof(int)) return int.TryParse(element.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i) ? i : 0;
    if (itemType == typeof(float)) return float.TryParse(element.Value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var f) ? f : 0f;
    if (itemType.IsEnum)
    {
      try { return Enum.Parse(itemType, element.Value.Trim(), ignoreCase: true); } catch { return null; }
    }
    if (itemType == typeof(Vector2)) return element.ToVector2();
    if (itemType == typeof(Rectangle)) return element.ToRectangle();
    if (itemType == typeof(Color)) return element.ToColor();

    if (typeof(Core.GameObject).IsAssignableFrom(itemType))
    {
      var typeAttr = element.Attribute("Type")?.Value;
      var t = ResolveType(typeAttr ?? itemType.FullName!, typeof(Core.GameObject)) ?? itemType;
      var go = Activator.CreateInstance(t) as Core.GameObject;
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
