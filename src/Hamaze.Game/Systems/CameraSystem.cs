using System.Numerics;
using Hamaze.Engine.Core;

namespace Hamaze.Game.Systems;

public class CameraSystem : WorldSystem
{
  IEntity? target = null;

  public override void Initialize(IEnumerable<IEntity> entities)
  {
    target = entities.FirstOrDefault(e => e.Tags.Contains("player"));
  }

  public override void Update(float DeltaTime, IEnumerable<IEntity> Entities)
  {
    if (target == null) return;
    Viewport.SetTarget(target.Position);
  }
}
