using System.Numerics;
using Hmk.Engine.Collision;
using Hmk.Engine.Core;
using Hmk.Engine.Graphics;
using Hmk.Engine.Input;
using Hmk.Engine.Serializer;

namespace Hmk.Game.Components.Player;

public class PlayerMovement : GameObject
{
  public new DynamicObject? Parent => base.Parent as DynamicObject;

  AnimationController? controller = null;

  [Save]
  public float Speed { get; set; } = 100f;

  public override void Initialize()
  {
    base.Initialize();
    ArgumentNullException.ThrowIfNull(Parent, nameof(Parent));

    controller = Parent.GetChild<AnimationController>();
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
      if (direction.X > 0)
      {
        Parent.FacingDirection = Directions.Right;
        controller?.PlayAnimation("WalkRight");
      }
      else if (direction.X < 0)
      {
        Parent.FacingDirection = Directions.Left;
        controller?.PlayAnimation("WalkLeft");
      }
      else if (direction.Y > 0)
      {
        Parent.FacingDirection = Directions.Down;
        controller?.PlayAnimation("WalkDown");
      }
      else if (direction.Y < 0)
      {
        Parent.FacingDirection = Directions.Up;
        controller?.PlayAnimation("WalkUp");
      }
    }
    else
    {
      // Play idle animation based on facing direction
      switch (Parent.FacingDirection)
      {
        case Directions.Right:
          controller?.PlayAnimation("IdleRight");
          break;
        case Directions.Left:
          controller?.PlayAnimation("IdleLeft");
          break;
        case Directions.Up:
          controller?.PlayAnimation("IdleUp");
          break;
        case Directions.Down:
        default:
          controller?.PlayAnimation("IdleDown");
          break;
      }
    }
    var velocity = direction * currentSpeed * dt;

    Parent.Move(velocity);
  }
}