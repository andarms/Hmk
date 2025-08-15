using Hmk.Engine.Resources;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Systems.Attack;

public class SimpleDamage : Resource, IDamageCalculator
{
  [Save]
  public int Amount { get; set; } = 0;
  public float CriticalChance { get; set; } = 0.01f;
  public float CriticalMultiplier { get; set; } = 1.5f;

  private float currentCriticalChance = 0f;

  public int CalculateDamage()
  {
    currentCriticalChance += CriticalChance;
    if (Random.Shared.NextDouble() < currentCriticalChance)
    {
      currentCriticalChance = 0f;
      Console.WriteLine("Critical Hit!");
      return (int)(Amount * CriticalMultiplier);
    }
    return Amount;
  }
}
