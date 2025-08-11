using Hmk.Engine.Core;

namespace Hmk.Engine.Collision;

public static class CollisionGameObjectExtensions
{
  static Rectangle Empty = new(0, 0, 0, 0);

  public static void SetCollider(this GameObject gameObject, Collider collider)
  {
    gameObject.Collider = collider;
  }

  public static Rectangle Bounds(this GameObject gameObject)
  {
    if (gameObject.Collider == null) return Empty;

    return new Rectangle(
      gameObject.GlobalPosition.X + gameObject.Collider.Offset.X,
      gameObject.GlobalPosition.Y + gameObject.Collider.Offset.Y,
      gameObject.Collider.Size.X,
      gameObject.Collider.Size.Y
    );
  }
}
