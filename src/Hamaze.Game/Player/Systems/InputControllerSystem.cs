using System.Numerics;
using Hamaze.Engine.Core;

namespace Hamaze.Game.Player.Systems;

public class PlayerControllerSystem : EntitySystem
{
  public override void Update(float dt, IEntity entity)
  {
    entity.Position += new Vector2(1, 0) * dt;
    Console.WriteLine($"Entity Position: {entity.Position}");
  }
}