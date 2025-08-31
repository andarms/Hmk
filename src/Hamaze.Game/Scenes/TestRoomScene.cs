using System.Numerics;
using Hamaze.Engine.Core;

namespace Hamaze.Game.Scenes;

public class TestRoomScene : Scene
{
  public override void Initialize()
  {
    Entity player = Player.Player.Create(new(100, 100));
    world.AddChild(player);
    world.AddSystem(new MapDrawSystems());
    world.AddSystem(new GridDebugSystem());
    world.AddSystem(new CameraSystem());
  }

  public override void Draw()
  {
    ClearBackground(Color.Gray);
    BeginMode2D(Viewport.Camera);
    world.Draw();
    EndMode2D();
  }
}


class Terrain
{
  public Rectangle Filled { get; } = new(0, 0, 16, 16);
  public Rectangle Empty { get; } = new(16, 0, 16, 16);
  public Rectangle Corner { get; } = new(32, 0, 16, 16);
}


public class MapDrawSystems : WorldSystem
{
  readonly Terrain terrain = new();
  Vector2 size = new(48, 16);
  public override void Draw(IEnumerable<IEntity> _)
  {
    // Draw the map using the terrain rectangles
    for (int x = 0; x < size.X; x++)
    {
      for (int y = 0; y < size.Y; y++)
      {
        Vector2 position = new(x * 16, y * 16);
        if (x > 10 && x < 22 && y > 4 && y < 9)
        {
          DrawTextureRec(ResourcesManager.Textures["Tilesets/terrain_001"], terrain.Filled, position, Color.White);
          continue;
        }
        DrawTextureRec(ResourcesManager.Textures["Tilesets/terrain_001"], terrain.Empty, position, Color.White);
      }
    }
  }
}
