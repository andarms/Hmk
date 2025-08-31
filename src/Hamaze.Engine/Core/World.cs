

namespace Hamaze.Engine.Core;

public class World
{

  readonly List<Entity> entities = [];
  readonly List<WorldSystem> systems = [];

  public void AddChild(Entity child)
  {
    entities.Add(child);
  }

  public void AddSystem(WorldSystem system)
  {
    systems.Add(system);
  }


  public void Initialize()
  {
    foreach (var entity in entities)
    {
      entity.Initialize();
    }
    foreach (var system in systems)
    {
      system.Initialize();
    }
  }

  public void Update(float dt)
  {
    foreach (var entity in entities)
    {
      entity.Update(dt);
    }
    foreach (var system in systems)
    {
      system.Update(dt, entities);
    }
  }

  public void Draw()
  {
    foreach (var entity in entities)
    {
      entity.Draw();
    }
    foreach (var system in systems)
    {
      system.Draw(entities);
    }
  }

  public void Terminate()
  {
    foreach (var entity in entities)
    {
      entity.Terminate();
    }
    foreach (var system in systems)
    {
      system.Terminate();
    }
  }

}