using System.Numerics;

namespace Hamaze.Engine.Core;

public interface IReadOnlyEntity
{
  Vector2 Position { get; }
  Vector2 GlobalPosition { get; }
  IReadOnlyList<Entity> Children { get; }
  T? GetComponent<T>() where T : Component;
  T Require<T>() where T : Component;
}

public interface IEntity : IReadOnlyEntity
{
  new Vector2 Position { get; set; }
  void AddComponent<T>(T component) where T : Component;
  void AddChild(Entity child);
  void AddSystem(EntitySystem system);
}