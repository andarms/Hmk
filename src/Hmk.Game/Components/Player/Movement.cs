using System.Numerics;
using Hmk.Engine.Collision;
using Hmk.Engine.Core;
using Hmk.Engine.Input;
using Hmk.Engine.Serializer;

namespace Hmk.Game.Components.Player;

public class PlayerMovement : GameObject
{

  public new DynamicObject? Parent => base.Parent as DynamicObject;

  [Save]
  public float Speed { get; set; } = 100f;

  public override void Initialize()
  {
    base.Initialize();
    ArgumentNullException.ThrowIfNull(Parent, nameof(Parent));
  }

  public override void Update(float dt)
  {
    base.Update(dt);
    if (Parent == null) return;

    var direction = InputManager.GetVector("move_left", "move_right", "move_up", "move_down");

    var stickInput = InputManager.GetGamepadLeftStick();
    if (stickInput.Length() > 0.1f)
    {
      direction = stickInput;
    }

    float currentSpeed = Speed;
    if (InputManager.IsHeld("run"))
    {
      currentSpeed *= 2f;
    }



    if (direction != Vector2.Zero)
    {
      direction = Vector2.Normalize(direction);
    }
    var velocity = direction * currentSpeed * dt;

    Parent.Move(velocity);
  }
}