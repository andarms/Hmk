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
    player.AddTag("player");
    player.AddComponent(new DynamicBody());
    player.AddComponent(new Sprite
    {
      TextureName = "Sprites/TinyDungeon",
      Source = new Rectangle(16, 160, 16, 16),
      Anchor = new Vector2(8, 16),
    });
    player.AddSystem(new PlayerControllerSystem());
    return player;
  }
}
