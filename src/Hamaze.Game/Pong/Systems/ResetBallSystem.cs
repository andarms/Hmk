using System.Numerics;
using Hamaze.Engine.Core;
using Hamaze.Engine.Core.Events;
using Hamaze.Game.Pong.Components;

namespace Hamaze.Game.Pong;

public class ResetBallSystem : WorldSystem
{
  IEntity? entity;
  public override void Initialize(IEnumerable<IEntity> entities)
  {
    base.Initialize(entities);
    entity = entities.FirstOrDefault(e => e.HasComponent<Ball>());

    EventManager.Subscribe<ScoreEvent>(OnScoreEvent);
  }

  private void OnScoreEvent(ScoreEvent @event)
  {
    if (entity != null)
    {
      entity.Position = Viewport.ScreenSize / 2;
      var ball = entity.Require<Ball>();
      ball.Velocity = Vector2.Normalize(new(GetRandomValue(-10, 10), GetRandomValue(-10, 10)));
    }
  }
}
