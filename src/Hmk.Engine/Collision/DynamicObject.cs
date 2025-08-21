using System.Numerics;
using Hmk.Engine.Core;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Collision;

public class DynamicObject : GameObject
{
  [Save]
  public Directions FacingDirection { get; set; } = Directions.Down;

  public void Move(Vector2 movement)
  {
    Position = new Vector2(GlobalPosition.X + movement.X, GlobalPosition.Y);
    foreach (var collision in CollisionsManager.GetPotentialCollisions(this))
    {
      CollisionsManager.ResolveSolidCollision(this, collision, true, false);
    }
    Position = new Vector2(GlobalPosition.X, GlobalPosition.Y + movement.Y);
    foreach (var collision in CollisionsManager.GetPotentialCollisions(this))
    {
      CollisionsManager.ResolveSolidCollision(this, collision, false, true);
    }
  }
}
