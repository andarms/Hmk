using Hmk.Engine.Collision;
using Hmk.Engine.Core;
using Hmk.Engine.Graphics;
using Hmk.Engine.Scenes;
using Hmk.Engine.Serializer;
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
    // Console.WriteLine(player.Serialize());

    var player = GameObjectSerializerExtensions.LoadFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Objects\player.xml"));
    AddChild(player);
    var obj = GameObjectSerializerExtensions.LoadFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Objects\ghost.xml"));
    AddChild(obj);
  }


  public override void Draw()
  {
    BeginMode2D(Camera);
    base.Draw();
    EndMode2D();
    // Update logic for the gameplay scene
  }
}