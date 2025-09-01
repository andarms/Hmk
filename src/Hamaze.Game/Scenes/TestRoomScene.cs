using System.Numerics;
using Hamaze.Engine.Core;

namespace Hamaze.Game.Scenes;

public class TestRoomScene : Scene
{
  public override void Initialize()
  {
    Entity player = Player.Player.Create(new(100, 100));
    world.AddChild(player);
    world.AddSystem(new TmxMapSystem());
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