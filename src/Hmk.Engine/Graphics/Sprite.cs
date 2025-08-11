using System.Numerics;
using System.Xml.Linq;
using Hmk.Engine.Resources;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Graphics;

public class Sprite() : Resource
{
  public string TextureName { get; set; } = string.Empty;
  public Rectangle Source { get; set; }
  public Vector2 Anchor { get; set; } = new Vector2(0, 0);
  public Color Tint { get; set; } = Color.White;
  public float Rotation { get; set; } = 0;

  public override XElement Serialize()
  {
    var element = base.Serialize();
    element.Add(new XElement("TextureName", TextureName));
    element.Add(Source.Serialize("Source"));
    element.Add(Anchor.Serialize("Anchor"));
    element.Add(Tint.Serialize("Tint"));
    element.Add(Rotation.Serialize("Rotation"));
    return element;
  }
}