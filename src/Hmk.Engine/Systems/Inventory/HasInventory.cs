using Hmk.Engine.Core;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Systems.Inventory;

public class HasInventory : Trait
{
  [Save]
  public Inventory Inventory { get; } = new Inventory();
}