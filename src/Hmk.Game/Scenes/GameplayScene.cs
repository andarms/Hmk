using Hmk.Engine.Core;
using Hmk.Engine.Graphics;
using Hmk.Engine.Resources;
using Hmk.Engine.Scenes;

namespace Hmk.Game.Scenes;

public class GameplayScene : Scene
{
  public override Color BackgroundColor => Color.DarkGreen;

  public override void Initialize()
  {
    base.Initialize();
    GameObject test = new()
    {
      Position = new(100, 100)
    };
    Sprite sprite = new()
    {
      Texture = ResourceManager.Textures["Sprites/TinyDungeon"],
      Source = new(16, 160, 16, 16)
    };
    SpriteRenderer spriteRenderer = new()
    {
      Sprite = sprite
    };
    test.AddChild(spriteRenderer);
    AddChild(test);
  }

  public override void Update(float dt)
  {
    base.Update(dt);
  }

  public override void Draw()
  {
    base.Draw();
    DrawText("Gameplay Scene", 10, 10, 20, Color.White);
  }
}
