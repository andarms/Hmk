using System.Globalization;
using System.Xml.Linq;

namespace Hmk.Engine.Serializer;

public static class PrimitivesSerializerExtensions
{
  public static XElement Serialize(this float data, string tagName = "Float")
  {
    var element = new XElement(tagName);
    element.SetValue(data.ToString(CultureInfo.InvariantCulture));
    return element;
  }

  public static float ToFloat(this XElement element)
  {
    return float.Parse(element.Value, CultureInfo.InvariantCulture);
  }

  public static XElement Serialize(this int data, string tagName = "Int")
  {
    var element = new XElement(tagName);
    element.SetValue(data);
    return element;
  }

  public static int ToInt(this XElement element)
  {
    return int.Parse(element.Value, CultureInfo.InvariantCulture);
  }

  public static XElement Serialize(this string data, string tagName = "String")
  {
    var element = new XElement(tagName);
    element.SetValue(data);
    return element;
  }

  public static string ToStringValue(this XElement element)
  {
    return element.Value;
  }
}
