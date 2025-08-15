using Hmk.Engine.Core;
using Hmk.Engine.Resources;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Systems.Attack;

public class Health : Resource
{
  [Save]
  public int Max { get; set; } = 0;
  [Save]
  public int Current { get; set; } = 0;

  public bool IsDead => Current <= 0;
  public Signal OnDead { get; } = new();
  public Signal<int> HealthChanged { get; } = new();

  public void TakeDamage(int amount)
  {
    if (IsDead) return;

    Current -= amount;
    if (Current < 0)
    {
      Current = 0;
      OnDead.Emit();
    }
    HealthChanged.Emit(Current);
    Console.WriteLine($"Health: {Current}/{Max}");
  }

  public void Heal(int amount)
  {
    if (IsDead) return;

    Current += amount;
    if (Current > Max) { Current = Max; }
    HealthChanged.Emit(Current);
  }

  public override string ToString()
  {
    return $"Health: {Current}/{Max}";
  }
}