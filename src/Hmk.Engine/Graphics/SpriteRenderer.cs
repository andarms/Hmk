using System.Numerics;
using Hmk.Engine.Core;
using Hmk.Engine.Resources;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Graphics;

public class SpriteRenderer : GameObject
{
  [Save]
  public ISprite Sprite { get; set; } = null!;


  [Save]
  public Vector2 Size { get; set; } = Vector2.Zero;




  private Rectangle cachedDestination;
  private Vector2 lastGlobalPosition;
  private Vector2 cachedSize;

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
    if (lastGlobalPosition != GlobalPosition || cachedSize != Size)
    {
      var w = Size.X > 0 ? Size.X : Sprite.Source.Width;
      var h = Size.Y > 0 ? Size.Y : Sprite.Source.Height;
      cachedDestination = new Rectangle(GlobalPosition.X, GlobalPosition.Y, w, h);
      lastGlobalPosition = GlobalPosition;
      cachedSize = Size;
    }
    return cachedDestination;
  }

  public override void Draw()
  {
    Sprite.Draw(GetDestination());
  }
}