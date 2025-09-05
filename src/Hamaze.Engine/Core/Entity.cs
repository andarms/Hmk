using System.Numerics;

namespace Hamaze.Engine.Core;

public sealed class Entity : IEntity, IReadOnlyEntity
{
  public Vector2 Position { get; set; }
  public Vector2 GlobalPosition => Parent?.GlobalPosition + Position ?? Position;
  public Entity? Parent { get; set; } = null;
  readonly List<Entity> children = [];
  readonly List<EntitySystem> systems = [];
  readonly List<string> tags = [];
  readonly Dictionary<string, Component> components = [];
  public IReadOnlyList<Entity> Children => children.AsReadOnly();
  public IReadOnlyList<string> Tags => tags.AsReadOnly();

  public Collider Collider { get; private set; } = new EmptyCollider();

  public void AddComponent<T>(T component) where T : Component
  {
    components.Add(typeof(T).Name, component);
  }

  public bool HasComponent<T>() where T : Component
  {
    return components.ContainsKey(typeof(T).Name);
  }

  public T? GetComponent<T>() where T : Component
  {
    components.TryGetValue(typeof(T).Name, out var component);
    return component as T;
  }

  public T Require<T>() where T : Component
  {
    return GetComponent<T>() ?? throw new InvalidOperationException($"Component of type {typeof(T).Name} is required but not found.");
  }

  public void AddChild(Entity child)
  {
    child.Parent = this;
    children.Add(child);
  }

  public void AddSystem(EntitySystem system)
  {
    systems.Add(system);
  }

  public void AddTag(string tag)
  {
    if (!tags.Contains(tag)) tags.Add(tag);
  }

  public void SetCollider(Collider collider)
  {
    Collider = collider;
  }

  public void Initialize()
  {
    foreach (var component in components.Values)
    {
      component.Initialize(this);
    }
    foreach (var system in systems)
    {
      system.Initialize(this);
    }
  }

  public void Update(float dt)
  {

    foreach (var component in components.Values)
    {
      component.Update(dt, this);
    }
    foreach (var system in systems)
    {
      system.Update(dt, this);
    }
  }

  public void Draw()
  {
    foreach (var component in components.Values)
    {
      component.Draw(this);
    }
    foreach (var system in systems)
    {
      system.Draw(this);
    }
  }

  public void Terminate()
  {
    foreach (var component in components.Values)
    {
      component.Terminate();
    }
    foreach (var system in systems)
    {
      system.Terminate();
    }
  }


  public Rectangle Bounds => new(Collider.Offset + GlobalPosition, Collider.Size);
}
