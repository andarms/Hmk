using System.Numerics;
using System.Xml.Linq;
using Hmk.Engine.Resources;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Graphics;

public class Sprite() : Resource
{
  [Save]
  public string TextureName { get; set; } = string.Empty;
  [Save]
  public Rectangle Source { get; set; }
  [Save]
  public Vector2 Anchor { get; set; } = new Vector2(0, 0);
  [Save]
  public Color Tint { get; set; } = Color.White;
  [Save]
  public float Rotation { get; set; } = 0;

}