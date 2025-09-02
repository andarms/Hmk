using System.Numerics;
using Hamaze.Engine.Core;

namespace Hamaze.Game.Pong.Components;

public class PaddleSprite : Component
{
  public Vector2 Size = new(16, 96);

  public override void Initialize(IReadOnlyEntity entity)
  {
    AIPaddle aiPaddle = entity.Require<AIPaddle>();
    Size = aiPaddle.Size;
  }

  public override void Draw(IReadOnlyEntity entity)
  {
    base.Draw(entity);
    // Draw the paddle sprite
    DrawRectangleV(entity.GlobalPosition, Size, Color.White);
    DrawRectangleLinesEx(
      entity.Bounds,
      2,
      Color.Red
    );
  }
}
