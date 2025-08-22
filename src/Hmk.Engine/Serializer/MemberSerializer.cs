using System.Collections;
using System.Numerics;
using System.Reflection;
using System.Xml.Linq;
using Hmk.Engine.Core;
using Hmk.Engine.Resources;

namespace Hmk.Engine.Serializer;

public static class MemberSerializer
{
  public static XElement Serialize(this MemberInfo member, object? value)
  {
    ArgumentNullException.ThrowIfNull(member);

    string memberName = member.Name;
    if (value is null)
    {
      return new XElement(memberName);
    }

    // Handle lists/collections (but not string)
    if (value is IEnumerable enumerable && value is not string && value is not GameObject && value is not Resource)
    {
      var parent = new XElement(memberName);
      foreach (var item in enumerable)
      {
        if (item is null)
        {
          parent.Add(new XElement("Item"));
          continue;
        }

        // Special handling for Dictionary<,> entries (KeyValuePair<K,V>)
        var itemType = item.GetType();
        if (itemType.IsGenericType && itemType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
        {
          var keyProp = itemType.GetProperty("Key");
          var valueProp = itemType.GetProperty("Value");
          var keyObj = keyProp?.GetValue(item);
          var valObj = valueProp?.GetValue(item);

          var itemElement = new XElement("Item");
          if (keyObj != null)
          {
            itemElement.SetAttributeValue("Key", keyObj.ToString());
          }

          switch (valObj)
          {
            case GameObject goVal:
              itemElement.Add(goVal.Serialize());
              break;
            case Resource resVal:
              itemElement.Add(resVal.Serialize());
              break;
            case Vector2 vVal:
              itemElement.Add(vVal.Serialize("Value"));
              break;
            case Rectangle rVal:
              itemElement.Add(rVal.Serialize("Value"));
              break;
            case Color cVal:
              itemElement.Add(cVal.Serialize("Value"));
              break;
            case null:
              // nothing to add
              break;
            default:
              // Fallback to attribute or inner text for simple values
              itemElement.SetAttributeValue("Value", valObj.ToString());
              break;
          }

          parent.Add(itemElement);
        }
        else
        {
          switch (item)
          {
            case GameObject go:
              parent.Add(go.Serialize());
              break;
            case Resource res:
              parent.Add(res.Serialize());
              break;
            case Vector2 v:
              parent.Add(v.Serialize("Item"));
              break;
            case Rectangle r:
              parent.Add(r.Serialize("Item"));
              break;
            case Color c:
              parent.Add(c.Serialize("Item"));
              break;
            default:
              parent.Add(new XElement("Item", item.ToString()));
              break;
          }
        }
      }
      return parent;
    }

    // Special case: ResourceReference<T> wrapper -> emit a ResourceRef with Path
    var valueType = value.GetType();
    if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(ResourceReference<>))
    {
      var pathProp = valueType.GetProperty("Path");
      var path = pathProp?.GetValue(value)?.ToString() ?? string.Empty;
      var el = new XElement("ResourceRef");
      el.SetAttributeValue("Property", memberName);
      el.SetAttributeValue("Path", path);
      return el;
    }

    // If the value is a Resource instance that is present in the ResourcesManager by some key, prefer referencing
    if (value is Resource resInst)
    {
      var key = ResourcesManager.Resources.FirstOrDefault(kv => ReferenceEquals(kv.Value, resInst)).Key;
      if (!string.IsNullOrEmpty(key))
      {
        var el = new XElement("ResourceRef");
        el.SetAttributeValue("Property", memberName);
        el.SetAttributeValue("Path", key);
        return el;
      }
      // Fallback to full serialization if not found in registry
      return resInst.Serialize().WithAttribute("Property", memberName);
    }

    return value switch
    {
      GameObject gameObject =>
        gameObject
        .Serialize()
        .WithAttribute("Property", memberName),
      Vector2 vector => vector.Serialize(memberName),
      Rectangle rectangle => rectangle.Serialize(memberName),
      Color color => color.Serialize(memberName),
      _ => new XElement(memberName, value.ToString())
    };
  }

  // Helper extension for adding an attribute fluently
  private static XElement WithAttribute(this XElement element, string name, object? value)
  {
    element.SetAttributeValue(name, value);
    return element;
  }
}