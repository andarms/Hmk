using Hmk.Engine.Collision;
using Hmk.Engine.Core;
using Hmk.Engine.Graphics;
using Hmk.Engine.Scenes;
using Hmk.Engine.Serializer;

namespace Hmk.Game.Scenes;


public class GameplayScene : Scene
{
  public override Color BackgroundColor => Color.DarkGreen;

  public override void Initialize()
  {
    base.Initialize();
    // GameObject test = new()
    // {
    //   Position = new(100, 100)
    // };
    // Sprite sprite = new()
    // {
    //   TextureName = "Sprites/TinyDungeon",
    //   Source = new(16, 160, 16, 16)
    // };
    // SpriteRenderer spriteRenderer = new()
    // {
    //   Sprite = sprite
    // };
    // test.AddChild(spriteRenderer);
    // Collider collider = new()
    // {
    //   Size = new(16, 16),
    //   Offset = new(0, 0)
    // };
    // test.SetCollider(collider);
    // AddChild(test);

    var obj = GameObjectSerializerExtensions.LoadFromXml(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Objects\ghost.xml"));
    AddChild(obj);
  }


  public override void Update(float dt)
  {
    base.Update(dt);
    // Update logic for the gameplay scene
  }
}