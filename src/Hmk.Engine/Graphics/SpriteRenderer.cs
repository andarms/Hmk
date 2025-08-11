using System.Numerics;
using Hmk.Engine.Core;
using Hmk.Engine.Resources;

namespace Hmk.Engine.Graphics;

public class SpriteRenderer : GameObject
{
  public Sprite Sprite { get; set; } = null!;
  Texture2D Texture => ResourceManager.Textures[Sprite.TextureName];

  private Rectangle cachedDestination;
  private Vector2 lastGlobalPosition;

  public override void Initialize()
  {
    ArgumentNullException.ThrowIfNull(Sprite, nameof(Sprite));
    if (string.IsNullOrEmpty(Sprite.TextureName) || !ResourceManager.Textures.ContainsKey(Sprite.TextureName))
    {
      throw new ArgumentException("Sprite must have a valid TextureName.", nameof(Sprite));
    }
  }

  public Rectangle GetDestination()
  {
    if (lastGlobalPosition != GlobalPosition)
    {
      cachedDestination = new Rectangle(GlobalPosition.X, GlobalPosition.Y, Sprite.Source.Width, Sprite.Source.Height);
      lastGlobalPosition = GlobalPosition;
    }
    return cachedDestination;
  }

  public override void Draw()
  {
    DrawTexturePro(Texture, Sprite.Source, GetDestination(), Sprite.Anchor, Sprite.Rotation, Sprite.Tint);
  }
}