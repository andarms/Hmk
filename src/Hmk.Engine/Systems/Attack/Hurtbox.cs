using System.Numerics;
using Hmk.Engine.Collision;
using Hmk.Engine.Core;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Systems.Attack;

public class Hurtbox : GameObject
{
  [Save]
  public Health Health { get; set; } = new();

  [Save]
  public TriggerZone Zone { get; set; } = new();

  public Hurtbox()
  {
  }

  public override void Initialize()
  {
    base.Initialize();
    ArgumentNullException.ThrowIfNull(Health, nameof(Health));

    Zone.OnEnter.Connect(HandleCollision);
    if (!this.HasTrait<HasTriggerZone>())
    {
      this.AddTrait(new HasTriggerZone { Zone = Zone });
    }
  }

  private void HandleCollision(GameObject other)
  {
    Console.WriteLine($"Hurtbox collided with {other.Name}");
    if (other is Hitbox hitbox)
    {
      Health.TakeDamage(hitbox.DamageCalculator.CalculateDamage());
    }
  }


  public override void Draw()
  {
    base.Draw();
    DrawTextEx(
      GetFontDefault(),
      $"Health: {Health.Current}/{Health.Max}", GlobalPosition + new Vector2(0, -8), 8, 1, Color.White);
  }
}


