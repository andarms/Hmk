using Hmk.Engine.Collision;
using Hmk.Engine.Core;
using Hmk.Engine.Graphics;
using Hmk.Engine.Scenes;
using Hmk.Engine.Serializer;
using Hmk.Engine.Systems.Attack;
using Hmk.Engine.Systems.Inventory;
using Hmk.Game.Components.Player;

namespace Hmk.Game.Scenes;

public class GameplayScene : Scene
{
  public override Color BackgroundColor => Color.DarkGreen;


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
  }


  public override void Draw()
  {
    BeginMode2D(Camera);
    base.Draw();
    EndMode2D();
  }
}