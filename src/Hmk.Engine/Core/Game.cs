
using Hmk.Engine.Input;
using Hmk.Engine.Resources;
using Hmk.Engine.Scenes;

namespace Hmk.Engine.Core;

public class Game
{
  public static void Initialize()
  {
    InitWindow(1280, 720, "Game");
    SetTargetFPS(60);
    ResourceManager.Initialize();
    SceneManager.Initialize();
    InputMap.CreateDefault().Apply();
  }

  public static void Run()
  {
    while (!WindowShouldClose())
    {
      BeginDrawing();
      float dt = GetFrameTime();
      SceneManager.Update(dt);
      SceneManager.Draw();
      EndDrawing();
    }

    ResourceManager.Terminate();
    CloseWindow();
  }
}