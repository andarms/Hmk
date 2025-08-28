using Hmk.Engine.Collision;
using Hmk.Engine.Core;
using Hmk.Engine.Input;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Systems.Interaction;

public class InteractionTrigger : Component
{
  [Save]
  public Collider? Collider { get; set; }

  public void HandleInteractionInput(DynamicObject parent)
  {
    if (parent?.FacingDirection == null) return;

    if (InputManager.JustPressed("confirm"))
    {
      CollisionsManager.TriggerInteraction(parent, parent.FacingDirection);
    }
  }
}
