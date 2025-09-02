using System.Numerics;
using Hamaze.Engine.Core;
using Hamaze.Engine.Core.Events;
using Hamaze.Game.Pong.Components;

namespace Hamaze.Game.Pong.Systems;

public class BallMovementSystem() : EntitySystem
{
  public float speed = 300f;
  Vector2 offset = new(32, 0);
  public override void Update(float dt, IEntity entity)
  {
    base.Update(dt, entity);
    Ball ball = entity.Require<Ball>();
    entity.Position += ball.Velocity * ball.SpeedIncrease * speed * dt;
    if (entity.Position.X < offset.X || entity.Position.X > Viewport.ScreenSize.X - offset.X)
    {
      ball.Velocity.X *= -1;
      int playerId = entity.Position.X < offset.X ? 2 : 1;
      ball.SpeedIncrease = 1f;
      EventManager.Emit(new ScoreEvent(PlayerId: playerId));
    }
    if (entity.Position.Y < 0 || entity.Position.Y > Viewport.ScreenSize.Y)
    {
      ball.Velocity.Y *= -1;
    }
  }
}
