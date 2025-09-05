using System.Numerics;

namespace Hamaze.Engine.Core.Components;

public class Sprite : Component
{
  public string TextureName { get; set; } = string.Empty;
  public Rectangle Source { get; set; } = new Rectangle(0, 0, 0, 0);
  public Vector2 Anchor { get; set; } = Vector2.Zero;
  public float Rotation { get; set; } = 0f;
  public Vector2 Scale { get; set; } = Vector2.One;
  public Texture2D Texture => ResourcesManager.Textures[TextureName];
}
