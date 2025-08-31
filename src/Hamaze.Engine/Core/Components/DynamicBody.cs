using System.Numerics;

namespace Hamaze.Engine.Core.Components;

public class DynamicBody : Component
{
  public Vector2 Velocity { get; set; } = Vector2.Zero;

  public void Move(Vector2 movement)
  {
    Velocity += movement;
  }
}
