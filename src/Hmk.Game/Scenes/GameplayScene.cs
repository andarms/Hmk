using Hmk.Editor.Scenes;
using Hmk.Engine.Collision;
using Hmk.Engine.Core;
using Hmk.Engine.Graphics;
using Hmk.Engine.Input;
using Hmk.Engine.Resources;
using Hmk.Engine.Scenes;
using Hmk.Engine.Serializer;
using Hmk.Engine.Systems.Inventory;

namespace Hmk.Game.Scenes;

public class GameplayScene : Scene
{
  readonly GameObjectTemplate<DynamicObject> playerTemplate = new("player");
  readonly GameObjectTemplate<CollectableItem> swordTemplate = new("Collectables/sword");

  public override void Initialize()
  {
    base.Initialize();

    var obj = GameObjectSerializerExtensions.LoadFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Objects/ghost.xml"));
    AddChild(obj);

    var spikes = GameObjectSerializerExtensions.LoadFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Objects/spikes.xml"));
    AddChild(spikes);

    var sword = swordTemplate.Instantiate();
    AddChild(sword);

    var signPost = GameObjectSerializerExtensions.LoadFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Objects/sign-post.xml"));
    AddChild(signPost);

    var player = playerTemplate.Instantiate();
    AddChild(player);

    Console.WriteLine(player.GetType());
    SceneManager.AddScene(new InventoryScene(player));

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

    AnimationController? controller = player.GetChild<AnimationController>();
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