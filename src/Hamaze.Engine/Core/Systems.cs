namespace Hamaze.Engine.Core;

public abstract class WorldSystem
{
  public virtual void Initialize(IEnumerable<IEntity> entities) { }
  public virtual void Update(float DeltaTime, IEnumerable<IEntity> entities) { }
  public virtual void Draw(IEnumerable<IEntity> entities) { }
  public virtual void Terminate() { }
}

public abstract class EntitySystem
{
  public virtual void Initialize(IEntity entity) { }
  public virtual void Update(float DeltaTime, IEntity entity) { }
  public virtual void Draw(IReadOnlyEntity entity) { }
  public virtual void Terminate() { }
}