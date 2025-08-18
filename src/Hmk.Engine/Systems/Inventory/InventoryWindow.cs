using System.Numerics;
using Hmk.Engine.Core;
using Hmk.Engine.Graphics;

namespace Hmk.Engine.Systems.Inventory;

public class InventoryWindow : GameObject
{
  Vector2 grid = new(5, 3);
  public List<InventorySlot> Slots { get; } = [];
  public Vector2 WindowSize { get; private set; }

  Color backgroundColor = new(0, 0, 0, 150);

  public InventoryWindow()
  {
    int gap = 4;
    int padding = 6;
    WindowSize = new(grid.X * (InventorySlot.Size.X + gap) + padding * 2, grid.Y * (InventorySlot.Size.Y + gap) + padding * 2);
    GameObject window = new()
    {
      Position = new(0, 0),
    };
    NinePatchSprite ninePatch = new()
    {
      TextureName = "Sprites/Inventory/Window",
      Source = new(0, 0, 19, 19),
      Left = 7,
      Top = 7,
      Right = 7,
      Bottom = 7,
      Anchor = new(0),
      Tint = Color.White
    };
    SpriteRenderer spriteRenderer = new()
    {
      Sprite = ninePatch,
      Size = WindowSize
    };
    window.AddChild(spriteRenderer);
    this.AddChild(window);


    float x = 8;
    float y = 8;
    for (int i = 0; i < grid.X; i++)
    {
      for (int j = 0; j < grid.Y; j++)
      {
        InventorySlot slot = new()
        {
          Position = new(x, y)
        };
        window.AddChild(slot);
        Slots.Add(slot);
        y += InventorySlot.Size.Y + gap;
      }
      y = 9;
      x += InventorySlot.Size.X + gap;
    }
  }



  // public override void Initialize()
  // {
  //   base.Initialize();
  //   ArgumentNullException.ThrowIfNull(Inventory, nameof(Inventory));
  // }

  public void UpdateInventory(Inventory inventory)
  {
    foreach (var slot in Slots)
    {
      slot.Clear();
    }
    var items = inventory.GetItems();
    for (int i = 0; i < items.Count; i++)
    {
      if (i < Slots.Count)
      {
        Slots[i].AddItem(items[i]);
      }
    }
  }


  public override void Draw()
  {
    DrawRectangleV(Vector2.Zero, Viewport.GetSize(), backgroundColor);
    base.Draw();
  }
}