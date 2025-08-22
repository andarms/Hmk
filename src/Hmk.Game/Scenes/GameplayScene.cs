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

    var ninja = GameObjectSerializerExtensions.LoadFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Objects/ninja.xml"));
    AddChild(ninja);

    SceneManager.AddScene(new InventoryScene(player2));

    // SpriteSheet spriteSheet = new()
    // {
    //   TextureName = "Sprites/Actor/NinjaGreen/SpriteSheet",
    //   FrameWidth = 16,
    //   FrameHeight = 16
    // };
    // spriteSheet.SetFrame(4);
    // GameObject ninja = new()
    // {
    //   Position = new Vector2(10, 100)
    // };
    // ninja.AddChild(new SpriteRenderer { Sprite = spriteSheet });
    // Animation walkDown = new() { SpriteSheet = spriteSheet, Frames = [4, 8, 12, 8], Speed = 180f };
    // Animation walkUp = new() { SpriteSheet = spriteSheet, Frames = [5, 9, 13, 9], Speed = 180f };
    // Animation walkLeft = new() { SpriteSheet = spriteSheet, Frames = [6, 10, 14, 10], Speed = 180f };
    // Animation walkRight = new() { SpriteSheet = spriteSheet, Frames = [7, 11, 15, 11], Speed = 180f };
    // AnimationController animationController = new();
    // animationController.Animations.Add("WalkDown", walkDown);
    // animationController.Animations.Add("WalkUp", walkUp);
    // animationController.Animations.Add("WalkLeft", walkLeft);
    // animationController.Animations.Add("WalkRight", walkRight);
    // animationController.PlayAnimation("WalkRight");
    // ninja.AddChild(animationController);
    // AddChild(ninja);


    // Console.WriteLine(ninja.Serialize());
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