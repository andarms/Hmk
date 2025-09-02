namespace Hamaze.Engine.Core;

public abstract class WorldSystem
{
  public virtual void Initialize() { }
  public virtual void Update(float DeltaTime, IEnumerable<IEntity> Entities) { }
  public virtual void Draw(IEnumerable<IEntity> Entities) { }
  public virtual void Terminate() { }
}

public abstract class EntitySystem
{
  public virtual void Initialize(IEntity entity) { }
  public virtual void Update(float DeltaTime, IEntity entity) { }
  public virtual void Draw(IReadOnlyEntity entity) { }
  public virtual void Terminate() { }
}