using System.Numerics;

using Hmk.Engine.Resources;

namespace Hmk.Engine.Graphics;

public class Sprite : Resource
{
  public Texture2D Texture { get; set; }
  public Rectangle Source { get; set; }
  public Vector2 Anchor { get; set; } = new Vector2(0, 0);
  public Color Tint { get; set; } = Color.White;
  public float Rotation { get; set; } = 0;
}