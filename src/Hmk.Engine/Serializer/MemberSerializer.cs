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
      return parent;
    }

    return value switch
    {
      GameObject gameObject =>
        gameObject
        .Serialize()
        .WithAttribute("Property", memberName),
      Resource resource =>
        resource
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