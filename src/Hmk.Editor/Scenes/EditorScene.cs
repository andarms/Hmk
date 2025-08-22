using Hmk.Engine.Scenes;
using ImGuiNET;
using rlImGui_cs;

namespace Hmk.Editor.Scenes;

public class EditorScene : Scene
{
  bool showDemo = true;

  public override void OnEnter()
  {
    base.OnEnter();
    rlImGui.Setup(false);
  }

  public override void OnExit()
  {
    base.OnExit();
    rlImGui.Shutdown();
  }

  public override void Update(float dt)
  {
    // basic input handling could go here
    base.Update(dt);
  }

  public override void Draw()
  {
    // Let base draw layers first
    base.Draw();

    ClearBackground(Color.Black);

    // Begin ImGui frame between BeginDrawing and EndDrawing in Game.Draw
    rlImGui.Begin();

    // Main dockspace window
    ImGui.Begin("Hmk Editor", ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoDocking);

    if (ImGui.BeginMenuBar())
    {
      if (ImGui.BeginMenu("File"))
      {
        if (ImGui.MenuItem("Exit"))
        {
          // close window via Raylib
          CloseWindow();
        }
        ImGui.EndMenu();
      }
      if (ImGui.BeginMenu("View"))
      {
        ImGui.MenuItem("ImGui Demo", null, ref showDemo);
        ImGui.EndMenu();
      }
      ImGui.EndMenuBar();
    }

    // Dockspace placeholder content
    ImGui.Text("Welcome to Hmk Editor");
    ImGui.Text("Use this window as a starting point.");

    ImGui.End();

    if (showDemo)
    {
      ImGui.ShowDemoWindow(ref showDemo);
    }

    rlImGui.End();
  }
}
