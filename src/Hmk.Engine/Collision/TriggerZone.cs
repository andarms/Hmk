using Hmk.Engine.Core;
using Hmk.Engine.Resources;

namespace Hmk.Engine.Collision;

public class TriggerZone : Resource
{
  public Signal<GameObject> OnEnter = new();
  public Signal<GameObject> OnExit = new();
}
