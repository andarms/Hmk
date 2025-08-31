using Hamaze.Engine.Core;

namespace Hamaze.Game.Scenes;

public class GridDebugSystem : WorldSystem
{
  public override void Draw(IEnumerable<IEntity> _)
  {
    // Draw grid lines
    for (int x = 0; x < Viewport.Width; x += 16)
    {
      DrawLine(x, 0, x, Viewport.Height, Color.LightGray);
    }
    for (int y = 0; y < Viewport.Height; y += 16)
    {
      DrawLine(0, y, Viewport.Width, y, Color.LightGray);
    }
  }
}