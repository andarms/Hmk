using System.Xml;

namespace Hamaze.Engine.Core;

public abstract class Component
{
  public virtual void Initialize() { }
  public virtual void Update(float dt, IReadOnlyEntity entity) { }
  public virtual void Draw(IReadOnlyEntity entity) { }
  public virtual void Terminate() { }
}
