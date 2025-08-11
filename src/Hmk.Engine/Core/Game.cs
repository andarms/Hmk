using Hmk.Engine.Input;
using Hmk.Engine.Scenes;

namespace Hmk.Engine.Core;

public class Game
{
  public static void Run()
  {
    InitWindow(1280, 720, "Game");
    SetTargetFPS(60);

    SceneManager.Initialize();
    InputMap.CreateDefault().Apply();

    while (!WindowShouldClose())
    {
      BeginDrawing();
      float dt = GetFrameTime();
      SceneManager.Update(dt);
      SceneManager.Draw();
      EndDrawing();
    }

    CloseWindow();
  }
}