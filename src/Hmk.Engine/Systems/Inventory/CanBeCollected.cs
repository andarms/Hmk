using Hmk.Engine.Core;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Systems.Inventory;

public class CanBeCollected : Trait
{
  [Save]
  public Item? Item { get; set; } = null;

  [Save]
  public bool AutoCollectionAllowed { get; set; } = true;
}
