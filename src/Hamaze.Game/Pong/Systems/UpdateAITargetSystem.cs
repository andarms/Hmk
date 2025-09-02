using System.Numerics;
using Hamaze.Engine.Core;
using Hamaze.Game.Pong.Components;

namespace Hamaze.Game.Pong.Systems;

public class UpdateAITargetSystem : WorldSystem
{
  public override void Update(float dt, IEnumerable<IEntity> entities)
  {
    base.Update(dt, entities);
    var ball = entities.FirstOrDefault(e => e.HasComponent<Ball>());
    var paddles = entities.Where(e => e.HasComponent<AIPaddle>());

    if (ball == null || !paddles.Any())
    {
      return;
    }

    foreach (var paddle in paddles)
    {
      AIPaddle ai = paddle.Require<AIPaddle>();
      ai.TargetPosition = ball.Position;
    }

  }
}
