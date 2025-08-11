using System.Numerics;

namespace Hmk.Engine.Collision;

public class Collider(Vector2 offset, Vector2 size)
{

  public Vector2 Offset { get; set; } = offset;
  public Vector2 Size { get; set; } = size;

  public Color DebugColor { get; set; } = new Color(0, 255, 0, 128);
  public Color DebugOutlineColor { get; set; } = new Color(0, 0, 0, 255);

  public Collider() : this(Vector2.Zero, Vector2.Zero)
  {
  }
}
