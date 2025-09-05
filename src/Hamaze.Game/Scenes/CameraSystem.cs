using System.Numerics;
using Hamaze.Engine.Core;
using Hamaze.Game.Player.Systems;

namespace Hamaze.Game.Scenes;

public class CameraSystem() : WorldSystem
{
  Vector2 camera = new(0, 0);
  IEntity? target = null;

  public override void Initialize(IEnumerable<IEntity> _)
  {
    target = _.FirstOrDefault(e => e.Tags.Contains("player"));
  }

  public override void Update(float DeltaTime, IEnumerable<IEntity> Entities)
  {
    if (target == null) return;

    // Smoothly interpolate camera towards target
    // float smoothness = 10f; // Higher is smoother
    // camera = Vector2.Lerp(camera, target.Position, DeltaTime * smoothness);
    camera = target.Position;

    Viewport.SetTarget(camera);
  }
}
