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