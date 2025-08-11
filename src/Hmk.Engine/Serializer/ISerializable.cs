using System.Xml.Linq;

namespace Hmk.Engine.Serializer;

public interface ISerializable
{
  XElement Serialize();
  void Deserialize(XElement element);
}
