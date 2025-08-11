using System.Xml.Linq;
using Hmk.Engine.Resources;

namespace Hmk.Engine.Core;

public class Trait : ISerializable
{
  public string Type => GetType().Name;

  public virtual XElement Serialize()
  {
    XElement element = new("Trait");
    element.SetAttributeValue("Type", Type);
    return element;
  }

  public virtual void Deserialize(XElement element)
  {
    ArgumentNullException.ThrowIfNull(element);
  }
}
