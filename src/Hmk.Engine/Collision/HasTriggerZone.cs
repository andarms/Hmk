using Hmk.Engine.Core;

namespace Hmk.Engine.Collision;

public class HasTriggerZone() : Trait
{
  public TriggerZone Zone { get; set; } = new TriggerZone();
}
