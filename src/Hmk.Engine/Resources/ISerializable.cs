using System.Xml.Linq;

namespace Hmk.Engine.Resources;

public interface ISerializable
{
  XElement Serialize();
  void Deserialize(XElement element);
}
