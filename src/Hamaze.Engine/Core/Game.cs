namespace Hamaze.Engine.Core;

public class Game
{
  public static void Initialize()
  {
    InitWindow(Settings.WindowWidth, Settings.WindowHeight, Settings.WindowTitle);
    SetTargetFPS(Settings.TargetFPS);

    ResourcesManager.Initialize();
    // CollisionsManager.Initialize();
    // SceneManager.Initialize();
    // var maps = InputMap.CreateDefault();
    // maps.AddAction("Inventory").WithKey(KeyboardKey.I);
    // maps.Apply();
  }

  public static void Run()
  {
    while (!WindowShouldClose())
    {
      Update();
      Draw();
    }

    ResourcesManager.Terminate();
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
    // CollisionsManager.Update();
    SceneManager.Update(dt);
    // DebugManager.Update(dt);
  }
}