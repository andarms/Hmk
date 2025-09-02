using System.Numerics;
using Hamaze.Engine.Core;

namespace Hamaze.Game.Pong.Components;

public class PaddleSprite : Component
{


  public override void Draw(IReadOnlyEntity entity)
  {
    base.Draw(entity);
    // Draw the paddle sprite
    DrawRectangleV(entity.GlobalPosition, AIPaddle.Size, Color.White);
    DrawRectangleLinesEx(
      entity.Bounds,
      2,
      Color.Red
    );
  }
}
