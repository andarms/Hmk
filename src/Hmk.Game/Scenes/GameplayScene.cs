using Hmk.Engine.Core;
using Hmk.Engine.Scenes;
using Raylib_cs;
using static Raylib_cs.Raylib;

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
