using Hmk.Engine.Collision;
using Hmk.Engine.Debug;
using Hmk.Engine.Input;
using Hmk.Engine.Resources;
using Hmk.Engine.Scenes;

namespace Hmk.Engine.Core;

public class Game
{
  public static void Initialize()
  {
    InitWindow(Settings.WindowWidth, Settings.WindowHeight, Settings.WindowTitle);
    SetTargetFPS(Settings.TargetFPS);

    ResourceManager.Initialize();
    SceneManager.Initialize();
    CollisionsManager.Initialize();
    var maps = InputMap.CreateDefault();
    maps.AddAction("Inventory").WithKey(KeyboardKey.I);
    maps.Apply();



  }

  public static void Run()
  {
    while (!WindowShouldClose())
    {
      Update();
      Draw();
    }

    ResourceManager.Terminate();
    CloseWindow();
  }

  private static void Draw()
  {
    BeginDrawing();
    SceneManager.Draw();
    EndDrawing();
  }

  private static void Update()
  {
    float dt = GetFrameTime();
    CollisionsManager.Update();
    SceneManager.Update(dt);
    DebugManager.Update(dt);
  }
}