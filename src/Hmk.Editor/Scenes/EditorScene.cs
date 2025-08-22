using Hmk.Engine.Graphics;
using Hmk.Engine.Scenes;
using IconFonts;
using ImGuiNET;
using rlImGui_cs;

namespace Hmk.Editor.Scenes;

public class EditorScene : Scene
{
  private bool playing = false;

  public EditorScene() : base()
  {
  }

  public override void OnEnter()
  {
    base.OnEnter();
    rlImGui.Setup(false);
  }


  public override void Initialize()
  {
    base.Initialize();

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
    ClearBackground(Color.Black);
    rlImGui.Begin();
    DrawMainMenuAndToolbar();
    rlImGui.End();
  }

  private void DrawMainMenuAndToolbar()
  {

    if (ImGui.BeginMainMenuBar())
    {
      if (ImGui.BeginMenu($"{FontAwesome6.Folder} Project"))
      {
        if (ImGui.MenuItem("Exit", "ctrl+i"))
        {
          CloseWindow();
        }
        ImGui.EndMenu();
      }

      if (ImGui.BeginMenu($"{FontAwesome6.PenToSquare} Edit"))
      {
        ImGui.MenuItem("Undo", "ctrl+z");
        ImGui.MenuItem("Redo", "ctrl+y");
        ImGui.EndMenu();
      }
      if (ImGui.BeginMenu($"{FontAwesome6.ScrewdriverWrench} Tools"))
      {
        ImGui.MenuItem("Layers");
        ImGui.MenuItem("Scene View");
        ImGui.MenuItem("Inspector", "ctrl+i", false);
        ImGui.Separator();
        ImGui.MenuItem("ImGui Demo");
        ImGui.EndMenu();
      }
      if (ImGui.BeginMenu($"{FontAwesome6.Download} Export"))
      {
        ImGui.MenuItem("Export as");
        ImGui.EndMenu();
      }

      ImGui.SameLine(ImGui.GetWindowWidth() - 104);
      if (ImGui.Button(playing ? FontAwesome6.Pause : FontAwesome6.Play, new(20, 20)))
      {
        playing = !playing;
      }
      ImGui.SameLine(0, 1);
      if (ImGui.Button(FontAwesome6.Stop, new(20, 20)))
      {
        playing = false;
      }


      float fps = GetFPS();
      string fpsText = $"{fps:0} FPS";
      float menuWidth = ImGui.GetWindowWidth();
      float textWidth = ImGui.CalcTextSize(fpsText).X;
      ImGui.SameLine(menuWidth - textWidth - 10);
      ImGui.TextUnformatted(fpsText);
      ImGui.EndMainMenuBar();

      ImGui.SetNextWindowPos(new(0, 20), ImGuiCond.Always);
      ImGui.SetNextWindowSize(new(256, Viewport.Height - 20));
      ImGui.Begin("Toolbar", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar);

      if (ImGui.Button($"{FontAwesome6.Eye}", new(20, 20)))
      {
        // Handle asset management here
      }
      ImGui.SameLine(0, 1);
      if (ImGui.Button($"Game Objects", new(198, 20)))
      {
        // Handle asset deletion here
      }
      ImGui.SameLine(0, 1);
      if (ImGui.Button($"{FontAwesome6.Plus}", new(20, 20)))
      {
        // Handle asset deletion here
      }

      // Treeview of all aviable textures

      ImGui.End();
    }
  }
}
