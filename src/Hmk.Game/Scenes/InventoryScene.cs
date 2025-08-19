using Hmk.Engine.Core;
using Hmk.Engine.Graphics;
using Hmk.Engine.Input;
using Hmk.Engine.Scenes;
using Hmk.Engine.Systems.Inventory;

namespace Hmk.Game.Scenes;


// TODO: Refactor to use GameObjectManager/InstancesManager
public class InventoryScene : Scene
{
  public InventoryScene(GameObject player)
  {
    InventoryWindow inventoryWindow = new();
    float x = Viewport.X + Viewport.GetSize().X / 2 - inventoryWindow.WindowSize.X / 2;
    float y = Viewport.Y + Viewport.GetSize().Y / 2 - inventoryWindow.WindowSize.Y / 2;
    inventoryWindow.Position = new(x, y);
    AddChild(inventoryWindow, UILayer);

    HasInventory? i = player.Trait<HasInventory>();
    Console.WriteLine("Inventory initialized");
    i?.Inventory?.OnItemAdded.Connect((_) =>
    {
      Console.WriteLine("Item added to inventory");
      inventoryWindow.UpdateInventory(i.Inventory);
    });
    i?.Inventory.OnItemRemoved.Connect((_) =>
    {
      Console.WriteLine("Item removed from inventory");
      inventoryWindow.UpdateInventory(i.Inventory);
    });
  }

  public override void Initialize()
  {
    base.Initialize();
  }

  public override void Update(float dt)
  {
    base.Update(dt);
    if (InputManager.JustPressed("Inventory"))
    {
      SceneManager.PopScene();
    }
  }

  public override void Draw()
  {
    BeginMode2D(Viewport.Camera);
    base.Draw();
    EndMode2D();
  }
}