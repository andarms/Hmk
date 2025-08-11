using System.Xml.Linq;

namespace Hmk.Engine.Resources;

public abstract class Resource : ISerializable
{
  public void Deserialize(XElement element)
  {
    throw new NotImplementedException();
  }

  public XElement Serialize()
  {
    throw new NotImplementedException();
  }
}