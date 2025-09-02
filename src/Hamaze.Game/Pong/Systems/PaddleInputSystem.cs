using System.Numerics;
using Hamaze.Engine.Core;
using Hamaze.Game.Pong.Components;

namespace Hamaze.Game.Pong.Systems;

public class PaddleInputSystem(float speed) : EntitySystem
{
  Vector2 paddleSize;
  public override void Initialize(IEntity entity)
  {
    base.Initialize(entity);
    paddleSize = entity.Require<PaddleSprite>().Size;
  }

  public override void Update(float dt, IEntity entity)
  {
    base.Update(dt, entity);


    if (IsKeyDown(KeyboardKey.Up))
    {
      entity.Position = new(entity.Position.X, entity.Position.Y - speed * dt);
    }
    if (IsKeyDown(KeyboardKey.Down))
    {
      entity.Position = new(entity.Position.X, entity.Position.Y + speed * dt);
    }
    entity.Position = Vector2.Clamp(entity.Position, new Vector2(0, 0), Viewport.ScreenSize - paddleSize);
  }
}
