using Hamaze.Engine.Core;
using Hamaze.Engine.Player;

namespace Hamaze.Game.Scenes;

public class TestRoomScene : Scene
{
  public override void Initialize()
  {
    Entity player = Player.Create(new(100, 100));
    world.AddChild(player);
  }
}