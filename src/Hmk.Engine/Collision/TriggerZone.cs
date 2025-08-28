using Hmk.Engine.Core;

namespace Hmk.Engine.Collision;

public class TriggerZone : Component
{
  public Signal<GameObject> OnEnter = new();
  public Signal<GameObject> OnExit = new();
}
