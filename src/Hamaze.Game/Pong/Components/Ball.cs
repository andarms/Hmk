using System.Numerics;
using Hamaze.Engine.Core;

namespace Hamaze.Game.Pong.Components;

public class Ball : Component
{
  public Vector2 Velocity = Vector2.Zero;
  public float SpeedIncrease = 1f;

  public override void Initialize(IReadOnlyEntity entity)
  {
    base.Initialize(entity);
    Velocity = Vector2.Normalize(new(GetRandomValue(-10, 10), GetRandomValue(-10, 10)));
  }
}
