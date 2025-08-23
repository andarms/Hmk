using System.Numerics;
using Hmk.Engine.Graphics;
using Hmk.Engine.Resources;
using Hmk.Engine.Scenes;
using IconFonts;
using ImGuiNET;
using rlImGui_cs;

namespace Hmk.Editor.Scenes;

public class SpriteSheetEditorScene(SpriteSheet sheet) : Scene
{
  Texture2D texture;
  Vector2 position;

  public override void Initialize()
  {
    base.Initialize();
    rlImGui.Setup(true);
    Viewport.SetZoom(3);
    ResetSprite();
  }

  private void ResetSprite()
  {
    texture = ResourcesManager.Textures[sheet.TextureName];
    float x = Viewport.X + Viewport.GetSize().X / 2 - texture.Width / 2;
    float y = Viewport.Y + Viewport.GetSize().Y / 2 - texture.Height / 2;
    position = new Vector2(x, y);
  }

  public override void Draw()
  {
    ClearBackground(Color.Black);

    rlImGui.Begin();
    BeginMode2D(Viewport.Camera);

    base.Draw();

    DrawRectangleV(position, new Vector2(texture.Width, texture.Height), Color.Magenta);

    DrawTexturePro(
      texture,
      new Rectangle(0, 0, texture.Width, texture.Height),
      new Rectangle(position.X, position.Y, texture.Width, texture.Height), Vector2.Zero, 0, Color.White);


    int linesX = Math.Max(1, texture.Width / sheet.FrameWidth);
    int linesY = Math.Max(1, texture.Height / sheet.FrameHeight);

    float cellWidth = texture.Width / linesX;
    float cellHeight = texture.Height / linesY;

    // Draw vertical lines
    for (int x = 0; x <= linesX; x++)
    {
      float xPos = position.X + x * cellWidth;
      DrawLineV(new Vector2(xPos, position.Y), new Vector2(xPos, position.Y + texture.Height), Color.Green);
    }

    // Draw horizontal lines
    for (int y = 0; y <= linesY; y++)
    {
      float yPos = position.Y + y * cellHeight;
      DrawLineV(new Vector2(position.X, yPos), new Vector2(position.X + texture.Width, yPos), Color.Green);
    }

    DrawInspector();
    EndMode2D();
    rlImGui.End();
  }

  private void DrawInspector()
  {
    ImGui.SetNextWindowPos(new(0, 0));
    ImGui.SetNextWindowSize(new(256, Viewport.Height));
    ImGui.Begin("#Inspector", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar);

    ImGui.TextColored(new Vector4(1, 1, 1, 1), sheet.TextureName);
    ImGui.SameLine(226);
    if (ImGui.Button($"{FontAwesome6.Xmark}", new(20)))
    {
      SceneManager.PopScene();
    }
    ImGui.Spacing();
    ImGui.Spacing();
    ImGui.Spacing();
    ImGui.Spacing();

    ImGui.PushItemWidth(156);
    int width = Math.Max(1, sheet.FrameWidth);
    int height = Math.Max(1, sheet.FrameHeight);
    ImGui.TextColored(new Vector4(1, 1, 1, 1), "Frames Size");
    ImGui.Separator();
    ImGui.TextColored(new Vector4(0.8f, 0.8f, 0.8f, 1), "X:");
    ImGui.SameLine(90);
    ImGui.InputInt("##positionx", ref width, 8, 16);
    width = Math.Max(1, width);
    ImGui.TextColored(new Vector4(0.8f, 0.8f, 0.8f, 1), "Y:");
    ImGui.SameLine(90);
    ImGui.InputInt("##positiony", ref height, 8, 16);
    height = Math.Max(1, height);
    sheet.FrameWidth = width;
    sheet.FrameHeight = height;
    ImGui.PopItemWidth();


    ImGui.Spacing();
    ImGui.TextColored(new Vector4(1, 1, 1, 1), "Texture");
    ImGui.Separator();


    ImGui.PushItemWidth(240);
    if (ImGui.BeginCombo("##texture", sheet.TextureName))
    {
      foreach (var tex in ResourcesManager.Textures)
      {
        if (ImGui.Selectable(tex.Key))
        {
          if (tex.Value.Id == texture.Id) continue;
          texture = tex.Value;
          sheet.TextureName = tex.Key;
          position = Vector2.Zero;
          ResetSprite();
        }
      }
      ImGui.EndCombo();
    }

    ImGui.Spacing();
    ImGui.Spacing();
    ImGui.Spacing();
    ImGui.Spacing();


    if (ImGui.Button($"{FontAwesome6.FloppyDisk} Save", new(240, 20)))
    {
      Console.WriteLine(sheet.Serialize());
    }

    ImGui.End();
  }
}