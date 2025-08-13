
using Hmk.Engine.Collision;
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
    CollisionsManager.Initialize();
    InputMap.CreateDefault().Apply();
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
  }
}