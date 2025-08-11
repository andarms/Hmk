using System.Xml.Linq;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Resources;

public abstract class Resource : ISerializable
{
  public virtual void Deserialize(XElement element)
  {

  }

  public virtual XElement Serialize()
  {
    XElement element = new("Resource");
    element.SetAttributeValue("Type", GetType().FullName);
    return element;
  }
}