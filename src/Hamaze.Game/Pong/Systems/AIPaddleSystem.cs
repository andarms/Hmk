using System.Numerics;
using Hamaze.Engine.Core;
using Hamaze.Game.Pong.Components;

namespace Hamaze.Game.Pong.Systems;

public class AIPaddleSystem : EntitySystem
{
  public override void Update(float dt, IEntity entity)
  {
    base.Update(dt, entity);
    AIPaddle ai = entity.Require<AIPaddle>();
    Vector2 direction = Vector2.Normalize(ai.TargetPosition - entity.Position);
    entity.Position = new Vector2(entity.Position.X, entity.Position.Y + direction.Y * ai.Speed * dt);
    entity.Position = Vector2.Clamp(entity.Position, new Vector2(0, 0), Viewport.ScreenSize - AIPaddle.Size);
  }
}
