using System.Numerics;
using Hmk.Engine.Collision;
using Hmk.Engine.Core;
using Hmk.Engine.Graphics;
using Hmk.Engine.Input;
using Hmk.Engine.Scenes;
using Hmk.Engine.Serializer;
using Hmk.Engine.Systems.Attack;
using Hmk.Engine.Systems.Interaction;
using Hmk.Engine.Systems.Inventory;
using Hmk.Game.Components.Player;

namespace Hmk.Game.Scenes;

public class GameplayScene : Scene
{
  public override void Initialize()
  {
    base.Initialize();

    // DynamicObject player = new() { Position = new(10, 100) };
    // Sprite sprite = new()
    // {
    //   TextureName = "Sprites/TinyDungeon",
    //   Source = new(16, 112, 16, 16)
    // };
    // SpriteRenderer spriteRenderer = new()
    // {
    //   Sprite = sprite
    // };
    // player.AddChild(spriteRenderer);
    // Collider collider = new()
    // {
    //   Size = new(8),
    //   Offset = new(4, 8)
    // };
    // player.SetCollider(collider);
    // player.AddChild(new PlayerMovement());
    // AddChild(player);
    // player.AddTrait(new IsSolid());

    // Health health = new()
    // {
    //   Max = 100,
    //   Current = 100
    // };
    // Hurtbox hurtbox = new()
    // {
    //   Health = health
    // };
    // player.AddChild(hurtbox);
    // player.AddTrait(new HasInventory());
    // Console.WriteLine(player.Serialize());
    // InventoryWindow inventoryWindow = new();
    // float x = Viewport.X + Viewport.GetSize().X / 2 - inventoryWindow.WindowSize.X / 2;
    // float y = Viewport.Y + Viewport.GetSize().Y / 2 - inventoryWindow.WindowSize.Y / 2;
    // inventoryWindow.Position = new Vector2(x, y);
    // AddChild(inventoryWindow, UILayer);

    var player2 = GameObjectSerializerExtensions.LoadFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Objects/player.xml"));
    AddChild(player2);

    var obj = GameObjectSerializerExtensions.LoadFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Objects/ghost.xml"));
    AddChild(obj);

    var spikes = GameObjectSerializerExtensions.LoadFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Objects/spikes.xml"));
    AddChild(spikes);

    var sword = GameObjectSerializerExtensions.LoadFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Objects/Collectables/sword.xml"));
    AddChild(sword);

    var signPost = GameObjectSerializerExtensions.LoadFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Objects/sign-post.xml"));
    AddChild(signPost);

    SceneManager.AddScene(new InventoryScene(player2));

    // InteractionTrigger trigger = new();
    // trigger.SetCollider(new()
    // {
    //   Size = new(8, 8),
    //   Offset = new(0, 0)
    // });
    // player.AddChild(trigger);

    // Console.WriteLine(player.Serialize());


  }


  public override void Update(float dt)
  {
    base.Update(dt);
    if (InputManager.JustPressed("Inventory"))
    {
      SceneManager.PushScene<InventoryScene>();
    }
  }


  public override void Draw()
  {
    ClearBackground(Color.DarkGreen);
    DrawFPS(10, 10);
    BeginMode2D(Viewport.Camera);
    base.Draw();
    EndMode2D();
  }
}