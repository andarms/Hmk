using Hamaze.Engine.Core;

namespace Hamaze.Game.Pong.Components;

public class BallSprite : Component
{
  readonly int radius = 16;

  public override void Draw(IReadOnlyEntity entity)
  {
    base.Draw(entity);
    // Draw the ball sprite
    DrawCircleV(entity.GlobalPosition, radius, Color.White);
    DrawRectangleLinesEx(
      entity.Bounds,
      1,
      Color.Red
    );
  }
}
