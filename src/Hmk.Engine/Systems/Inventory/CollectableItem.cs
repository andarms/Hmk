using Hmk.Engine.Core;
using Hmk.Engine.Graphics;

namespace Hmk.Engine.Systems.Inventory;

public class CollectableItem : GameObject
{
  SpriteRenderer Renderer { get; set; } = null!;

  public override void Initialize()
  {
    base.Initialize();

    var trait = this.Trait<CanBeCollected>();
    if (trait == null)
    {
      trait = new CanBeCollected();
      this.AddTrait(trait);
    }
    ArgumentNullException.ThrowIfNull(trait.Item, "CanBeCollected trait must have an Item assigned.");
    ArgumentNullException.ThrowIfNull(trait.Item.Sprite, "Item assigned to CanBeCollected trait must have a sprite to be collected.");

    Renderer = new SpriteRenderer()
    {
      Sprite = trait.Item.Sprite
    };
    this.AddChild(Renderer);
  }
}