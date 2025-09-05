using System.Numerics;
using Hamaze.Engine.Core;

namespace Hamaze.Game.Player.Systems;

public class PlayerControllerSystem : EntitySystem
{
  public override void Update(float dt, IEntity entity)
  {
    Vector2 direction = Vector2.Zero;
    if (IsKeyDown(KeyboardKey.Up)) direction.Y -= 1;
    if (IsKeyDown(KeyboardKey.Down)) direction.Y += 1;
    if (IsKeyDown(KeyboardKey.Left)) direction.X -= 1;
    if (IsKeyDown(KeyboardKey.Right)) direction.X += 1;

    if (direction != Vector2.Zero)
    {
      direction = Vector2.Normalize(direction);
    }
    entity.Position += direction * 100 * dt;
  }
}