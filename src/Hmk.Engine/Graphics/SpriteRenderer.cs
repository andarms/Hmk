using System.Numerics;
using Hmk.Engine.Core;

namespace Hmk.Engine.Graphics;

public class SpriteRenderer : GameObject
{
  public Sprite Sprite { get; set; } = null!;


  private Rectangle cachedDestination;
  private Vector2 lastGlobalPosition;

  public override void Initialize()
  {
    ArgumentNullException.ThrowIfNull(Sprite, nameof(Sprite));
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
    DrawTexturePro(Sprite.Texture, Sprite.Source, GetDestination(), Sprite.Anchor, Sprite.Rotation, Sprite.Tint);
  }
}