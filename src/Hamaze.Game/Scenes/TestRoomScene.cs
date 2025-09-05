using System.Numerics;
using Hamaze.Engine.Core;
using Hamaze.Engine.Core.Systems;

namespace Hamaze.Game.Scenes;

public class TestRoomScene : Scene
{
  public override void Initialize()
  {
    Entity player = Player.Player.Create(new(16, 16));
    world.AddChild(player);


    world.AddSystem(new TmxMapSystem());
    world.AddSystem(new GridDebugSystem());
    world.AddSystem(new CameraSystem());
    world.AddSystem(new SpriteDrawSystem());
  }

  public override void Draw()
  {
    ClearBackground(Color.Gray);
    BeginMode2D(Viewport.Camera);
    world.Draw();
    EndMode2D();
  }
}