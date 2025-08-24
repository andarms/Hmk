using System.Numerics;
using Hmk.Editor.Scenes;
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

    Sprite sprite = new()
    {
      TextureName = "Sprites/TinyDungeon",
      Source = new(16, 112, 16, 16)
    };
    SceneManager.AddScene(new SpriteEditorScene(sprite));

    SpriteSheet sheet = new()
    {
      TextureName = "Sprites/TinyDungeon",
      FrameWidth = 16,
      FrameHeight = 16
    };
    SceneManager.AddScene(new SpriteSheetEditorScene(sheet));

    AnimationController? controller = player2.GetChild<AnimationController>();
    if (controller != null)
    {
      SceneManager.AddScene(new AnimationEditorScene(controller));
    }
  }


  public override void Update(float dt)
  {
    base.Update(dt);
    if (InputManager.JustPressed("Inventory"))
    {
      SceneManager.PushScene<InventoryScene>();
    }

    // Editor shortcuts:
    // F2 - Sprite Editor
    // F3 - SpriteSheet Editor
    // F4 - Animation Editor
    if (IsKeyPressed(KeyboardKey.F2))
    {
      SceneManager.PushScene<SpriteEditorScene>();
    }
    if (IsKeyPressed(KeyboardKey.F3))
    {
      SceneManager.PushScene<SpriteSheetEditorScene>();
    }
    if (IsKeyPressed(KeyboardKey.F4))
    {
      SceneManager.PushScene<AnimationEditorScene>();
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