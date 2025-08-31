using System.Numerics;
using Hamaze.Engine.Core;
using Hamaze.Engine.Core.Components;
using Hamaze.Game.Player.Systems;

namespace Hamaze.Game.Player;

public class Player
{
  public static Entity Create(Vector2 position)
  {
    Entity player = new()
    {
      Position = position
    };
    player.AddComponent(new DynamicBody());
    player.AddSystem(new PlayerControllerSystem());
    return player;
  }
}
