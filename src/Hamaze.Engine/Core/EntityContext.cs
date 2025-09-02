using System.Numerics;

namespace Hamaze.Engine.Core;

public class Collider(Vector2 offset, Vector2 size)
{

  public Vector2 Offset { get; set; } = offset;
  public Vector2 Size { get; set; } = size;
}


public class EmptyCollider : Collider
{
  public EmptyCollider() : base(Vector2.Zero, Vector2.Zero)
  {
  }
}

public interface IReadOnlyEntity
{
  Vector2 Position { get; }
  Vector2 GlobalPosition { get; }
  IReadOnlyList<Entity> Children { get; }
  T? GetComponent<T>() where T : Component;
  T Require<T>() where T : Component;
  bool HasComponent<T>() where T : Component;
  Collider Collider { get; }
  Rectangle Bounds { get; }
}

public interface IEntity : IReadOnlyEntity
{
  new Vector2 Position { get; set; }
  void AddComponent<T>(T component) where T : Component;
  void AddChild(Entity child);
  void AddSystem(EntitySystem system);
  void SetCollider(Collider collider);
}