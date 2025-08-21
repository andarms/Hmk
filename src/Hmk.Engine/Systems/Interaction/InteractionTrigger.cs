using Hmk.Engine.Collision;
using Hmk.Engine.Core;
using Hmk.Engine.Input;

namespace Hmk.Engine.Systems.Interaction;

public class InteractionTrigger : GameObject
{
  public new DynamicObject? Parent => base.Parent as DynamicObject;

  public override void Initialize()
  {
    base.Initialize();
    ArgumentNullException.ThrowIfNull(Parent, nameof(Parent));
  }

  public override void Update(float dt)
  {
    base.Update(dt);
    if (Parent?.FacingDirection == null) return;

    UpdatePosition();
    if (InputManager.JustPressed("confirm"))
    {
      CollisionsManager.TriggerInteraction(this, Parent.FacingDirection);
    }
  }

  private void UpdatePosition()
  {
    if (Parent?.Collider == null) return;
    if (Collider == null) return;

    var playerCenter = Parent.Collider.Offset + Parent.Collider.Size * 0.5f;
    // Calculate the offset in the facing direction, placing the interaction area just outside the player
    var facingOffset = Parent.FacingDirection.ToVector2() * (Parent.Collider.Size.Y * 0.5f + Collider.Size.Y * 0.5f);
    Position = playerCenter + facingOffset - Collider.Size * 0.5f;
  }
}
