using Hmk.Engine.Core;

namespace Hmk.Engine.Collision;

public static class CollisionGameObjectExtensions
{
  static Rectangle Empty = new(0, 0, 0, 0);

  public static void SetCollider(this GameObject gameObject, Collider collider)
  {
    gameObject.Collider = collider;
    CollisionsManager.AddObject(gameObject);
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


  public static void DebugCollider(this GameObject gameObject)
  {
    if (gameObject.Collider == null) return;
    DrawRectangleV(
      gameObject.GlobalPosition + gameObject.Collider.Offset,
      gameObject.Collider.Size,
      gameObject.Collider.DebugColor
    );
    DrawRectangleLinesEx(
      gameObject.Bounds(),
      1,
      gameObject.Collider.DebugOutlineColor
    );
  }

  public static bool Collides(this GameObject a, GameObject b)
  {
    return CheckCollisionRecs(a.Bounds(), b.Bounds());
  }

  public static bool Equals(this Rectangle a, Rectangle b)
  {
    return a.X == b.X && a.Y == b.Y && a.Width == b.Width && a.Height == b.Height;
  }
}
