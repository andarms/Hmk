
using System.Numerics;
using Hamaze.Engine.Core.Components;

namespace Hamaze.Engine.Core.Systems;

public class SpriteDrawSystem : WorldSystem
{
  public override void Draw(IEnumerable<IEntity> entities)
  {
    base.Draw(entities);
    var sprites = entities.Where(e => e.HasComponent<Sprite>());
    sprites = sprites.OrderBy(e => e.Position.Y);
    foreach (var entity in sprites)
    {
      Sprite sprite = entity.Require<Sprite>();
      DrawTexturePro(
        sprite.Texture,
        sprite.Source,
        new Rectangle(
          entity.GlobalPosition.X * sprite.Scale.X,
          entity.GlobalPosition.Y * sprite.Scale.Y,
          sprite.Source.Width * sprite.Scale.X,
          sprite.Source.Height * sprite.Scale.Y
        ),
        sprite.Anchor * sprite.Scale,
        sprite.Rotation,
        Color.White
      );


    }
  }
}