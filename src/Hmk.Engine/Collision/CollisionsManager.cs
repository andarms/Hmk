using Hmk.Engine.Core;

namespace Hmk.Engine.Collision;

public static class CollisionsManager
{

  const int CELL_SIZE = 64;
  const int WORLD_WIDTH = 10_000;
  const int WORLD_HEIGHT = 10_000;

  public static SpatialGrid Grid { get; private set; } = new SpatialGrid(CELL_SIZE, WORLD_WIDTH, WORLD_HEIGHT);


  private static readonly List<GameObject> objects = [];
  private static readonly Dictionary<GameObject, Rectangle> lastKnownCollisionBounds = [];

  public static void Initialize()
  {
    Grid = new SpatialGrid(CELL_SIZE, WORLD_WIDTH, WORLD_HEIGHT);
    objects.Clear();
    lastKnownCollisionBounds.Clear();
  }


  public static void AddObject(GameObject obj)
  {
    if (!objects.Contains(obj))
    {
      objects.Add(obj);
      lastKnownCollisionBounds[obj] = obj.Bounds();
      Grid.AddObject(obj);
    }
  }

  public static void RemoveObject(GameObject obj)
  {
    if (objects.Remove(obj))
    {
      lastKnownCollisionBounds.Remove(obj);
    }
    Grid.RemoveObject(obj);
  }

  public static void Update()
  {
    UpdateGrid();
    CheckCollisions();
  }


  private static void UpdateGrid()
  {
    var objectsToUpdate = new List<(GameObject obj, Rectangle oldBounds)>();
    foreach (var obj in objects)
    {
      if (lastKnownCollisionBounds.TryGetValue(obj, out var lastKnownBounds))
      {
        var current = obj.Bounds();
        // Update spatial cell only when bounds have changed
        if (!lastKnownBounds.Equals(current))
        {
          objectsToUpdate.Add((obj, lastKnownBounds));
        }
      }
    }

    foreach (var (obj, _) in objectsToUpdate)
    {
      Grid.UpdateObject(obj);
      lastKnownCollisionBounds[obj] = obj.Bounds();
    }
  }


  public static void CheckCollisions()
  {
    var checkedPairs = new HashSet<(GameObject, GameObject)>();
    foreach (var objA in objects.ToList())
    {
      foreach (var objB in Grid.GetPotentialCollisions(objA))
      {
        var pair = objA.GetHashCode() < objB.GetHashCode() ? (objA, objB) : (objB, objA);
        if (checkedPairs.Contains(pair)) { continue; }
        checkedPairs.Add(pair);

        bool wasColliding = objA.Collisions.Contains(objB);
        bool isColliding = objA.Collides(objB);

        TriggerZone? zoneA = objA.Trait<HasTriggerZone>()?.Zone;
        TriggerZone? zoneB = objB.Trait<HasTriggerZone>()?.Zone;

        if (isColliding && !wasColliding)
        {
          objA.Collisions.Add(objB);
          objB.Collisions.Add(objA);

          zoneA?.OnEnter.Emit(objB);
          zoneB?.OnEnter.Emit(objA);
          // HandleCollisionPickup(objA, objB);
        }
        else if (!isColliding && wasColliding)
        {
          objA.Collisions.Remove(objB);
          objB.Collisions.Remove(objA);
          zoneA?.OnExit.Emit(objB);
          zoneB?.OnExit.Emit(objA);
        }
      }
    }
  }

  public static IEnumerable<GameObject> GetPotentialCollisions(GameObject obj)
  {
    foreach (var nearby in Grid.GetPotentialCollisions(obj) ?? [])
    {
      if (nearby == null || obj == nearby) { continue; }
      if (nearby.Collider == null || obj.Collider == null) { continue; }
      if (nearby.Collides(obj))
      {
        yield return nearby;
      }
    }
  }


  public static void ResolveSolidCollision(GameObject obj, GameObject other, bool resolveX, bool resolveY)
  {
    if (obj.Collider == null || other.Collider == null) return;
    if (other.HasTrait<IsSolid>())
    {
      StopObject(obj, other, resolveX, resolveY);
    }
  }

  private static void StopObject(GameObject obj, GameObject other, bool resolveX, bool resolveY)
  {
    if (obj.Collider == null || other.Collider == null) return;
    if (resolveX)
    {
      if (obj.Position.X < other.Position.X)
      {
        // Object is to the left, push it left
        obj.Position = new(other.Position.X - obj.Collider.Size.X - obj.Collider.Offset.X, obj.Position.Y);
      }
      else
      {
        // Object is to the right, push it right
        obj.Position = new(other.Position.X + other.Collider.Size.X - obj.Collider.Offset.X, obj.Position.Y);
      }
    }

    if (resolveY)
    {
      if (obj.Position.Y < other.Position.Y)
      {
        // Object is above, push it up
        obj.Position = new(obj.Position.X, other.Position.Y - other.Collider.Size.Y - obj.Collider.Offset.Y + obj.Collider.Offset.Y);
      }
      else
      {
        // Object is below, push it down
        obj.Position = new(obj.Position.X, other.Position.Y + other.Collider.Size.Y + other.Collider.Offset.Y - obj.Collider.Offset.Y);
      }
    }
  }
}
