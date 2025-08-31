using System.Numerics;

namespace Hamaze.Engine.Core;

public sealed class Entity : IEntity, IReadOnlyEntity
{
  public Vector2 Position { get; set; }
  public Vector2 GlobalPosition => Parent?.GlobalPosition + Position ?? Position;
  public Entity? Parent { get; set; } = null;
  readonly List<Entity> children = [];
  readonly List<EntitySystem> systems = [];
  readonly Dictionary<string, Component> components = [];
  public IReadOnlyList<Entity> Children => children.AsReadOnly();

  public void AddComponent<T>(T component) where T : Component
  {
    components.Add(typeof(T).Name, component);
  }

  public T? GetComponent<T>() where T : Component
  {
    return components[typeof(T).Name] as T;
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

  public void Initialize()
  {
    foreach (var component in components.Values)
    {
      component.Initialize();
    }
    foreach (var system in systems)
    {
      system.Initialize();
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
}
