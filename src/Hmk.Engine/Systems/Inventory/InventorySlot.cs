using System.Numerics;
using Hmk.Engine.Core;
using Hmk.Engine.Graphics;

namespace Hmk.Engine.Systems.Inventory;

public class InventorySlot : GameObject
{
  public static Vector2 Size { get; } = new(20, 20);

  public bool Occupied { get; set; } = false;

  readonly Sprite background = new()
  {
    TextureName = "Sprites/Inventory/Slot",
    Source = new(0, 0, 20, 20)
  };

  Rectangle emptySource = new(0, 0, 20, 20);
  Rectangle filledSource = new(0, 20, 20, 20);

  public InventorySlot()
  {
    background.Source = Occupied ? filledSource : emptySource;
    SpriteRenderer spriteRenderer = new()
    {
      Sprite = background,
    };
    this.AddChild(spriteRenderer);
  }



  public void AddItem(Item item)
  {

    ArgumentNullException.ThrowIfNull(item, nameof(item));
    ArgumentNullException.ThrowIfNull(item.Sprite, nameof(item.Sprite));

    Occupied = true;
    background.Source = filledSource;

    SpriteRenderer itemRenderer = new()
    {
      Sprite = item.Sprite,
      Position = new Vector2(2)
    };
    this.AddChild(itemRenderer);
  }

  public void Clear()
  {
    Occupied = false;
    background.Source = emptySource;
  }
}