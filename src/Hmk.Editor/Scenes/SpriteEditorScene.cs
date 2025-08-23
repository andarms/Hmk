using System.Numerics;
using Hmk.Engine.Graphics;
using Hmk.Engine.Input;
using Hmk.Engine.Resources;
using Hmk.Engine.Scenes;
using IconFonts;
using ImGuiNET;
using rlImGui_cs;

namespace Hmk.Editor.Scenes;

public class SpriteEditorScene : Scene
{
  Texture2D texture;
  Sprite sprite;
  Vector2 position;

  // Tint color picker popup state
  private bool tintPopupOpen = false;
  private Vector4 tempTint;

  public SpriteEditorScene(Sprite sprite)
  {
    this.sprite = sprite;
  }

  public override void Initialize()
  {
    base.Initialize();
    rlImGui.Setup(true);
    Viewport.SetZoom(3);
    ResetSprite();
  }

  private void ResetSprite()
  {
    texture = ResourcesManager.Textures[sprite.TextureName];
    float x = Viewport.X + Viewport.GetSize().X / 2 - texture.Width / 2;
    float y = Viewport.Y + Viewport.GetSize().Y / 2 - texture.Height / 2;
    position = new Vector2(x, y);
  }

  public override void Update(float dt)
  {
    base.Update(dt);
    if (InputManager.JustPressed("Inventory"))
    {
      SceneManager.PopScene();
    }
  }

  public Rectangle Selection => new(sprite.Source.X + position.X, sprite.Source.Y + position.Y, sprite.Source.Width, sprite.Source.Height);

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
      new Rectangle(position.X, position.Y, texture.Width, texture.Height), Vector2.Zero, 0, sprite.Tint);

    DrawRectangleLinesEx(Selection, 1, Color.Green);
    DrawRectangleRec(Selection, Fade(Color.Lime, 0.25f));

    DrawCircleV(sprite.Anchor + position + sprite.Source.Position, 2, Color.Red);

    DrawInspector();
    EndMode2D();
    rlImGui.End();
  }

  private void DrawInspector()
  {
    ImGui.SetNextWindowPos(new(0, 0));
    ImGui.SetNextWindowSize(new(256, Viewport.Height));
    ImGui.Begin("#Inspector", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar);

    ImGui.TextColored(new Vector4(1, 1, 1, 1), sprite.TextureName);
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

    sprite.Source = DrawRectangleInspector(sprite.Source);

    ImGui.Spacing();
    ImGui.TextColored(new Vector4(1, 1, 1, 1), "Texure");
    ImGui.Separator();
    ImGui.PopItemWidth();

    ImGui.PushItemWidth(240);
    if (ImGui.BeginCombo("##texture", sprite.TextureName))
    {
      foreach (var tex in ResourcesManager.Textures)
      {
        if (ImGui.Selectable(tex.Key))
        {
          if (tex.Value.Id == texture.Id) continue;
          texture = tex.Value;
          sprite.TextureName = tex.Key;
          position = Vector2.Zero;
          ResetSprite();
          sprite.Source = new(0, 0, sprite.Source.Width, sprite.Source.Height);
        }
      }
      ImGui.EndCombo();
    }
    ImGui.PopItemWidth();

    // Anchor
    ImGui.Spacing();
    float anchorX = sprite.Anchor.X;
    float anchorY = sprite.Anchor.Y;
    ImGui.TextColored(new Vector4(1, 1, 1, 1), "Anchor");
    ImGui.Separator();
    ImGui.TextColored(new Vector4(0.8f, 0.8f, 0.8f, 1), "X:");
    ImGui.SameLine(90);
    ImGui.InputFloat("##anchorx", ref anchorX, 8, 16, "%.2f");
    ImGui.TextColored(new Vector4(0.8f, 0.8f, 0.8f, 1), "Y:");
    ImGui.SameLine(90);
    ImGui.InputFloat("##anchory", ref anchorY, 8, 16, "%.2f");
    sprite.Anchor = new Vector2(anchorX, anchorY);

    // Tint Picker Modal
    ImGui.TextColored(new Vector4(1, 1, 1, 1), "Tint");
    ImGui.Separator();
    if (ImGui.Button("Edit Tint", new(240, 20)))
    {
      tempTint = new Vector4(sprite.Tint.R, sprite.Tint.G, sprite.Tint.B, sprite.Tint.A);
      tintPopupOpen = true;
      ImGui.OpenPopup("TintPickerPopup");
    }
    if (tintPopupOpen && ImGui.BeginPopupModal("TintPickerPopup", ImGuiWindowFlags.AlwaysAutoResize))
    {
      ImGui.ColorPicker4("##tintpicker", ref tempTint);
      if (ImGui.Button("OK", new(100, 20)))
      {
        sprite.Tint = new Color(tempTint.X, tempTint.Y, tempTint.Z, tempTint.W);
        tintPopupOpen = false;
        ImGui.CloseCurrentPopup();
      }
      ImGui.SameLine();
      if (ImGui.Button("Cancel", new(100, 20)))
      {
        tintPopupOpen = false;
        ImGui.CloseCurrentPopup();
      }
      ImGui.EndPopup();
    }

    ImGui.Spacing();
    ImGui.Spacing();
    ImGui.Spacing();
    ImGui.Spacing();


    if (ImGui.Button($"{FontAwesome6.FloppyDisk} Save", new(240, 20)))
    {
      Console.WriteLine(sprite.Serialize());
    }




    ImGui.End();
  }

  private Rectangle DrawRectangleInspector(Rectangle rectangle)
  {
    float x = rectangle.X;
    float y = rectangle.Y;
    float width = rectangle.Width;
    float height = rectangle.Height;

    ImGui.TextColored(new Vector4(1, 1, 1, 1), "Position");
    ImGui.Separator();
    ImGui.TextColored(new Vector4(0.8f, 0.8f, 0.8f, 1), "X:");
    ImGui.SameLine(90);
    ImGui.InputFloat("##positionx", ref x, 8, 16, "%.2f");
    ImGui.TextColored(new Vector4(0.8f, 0.8f, 0.8f, 1), "Y:");
    ImGui.SameLine(90);
    ImGui.InputFloat("##positiony", ref y, 8, 16, "%.2f");


    ImGui.TextColored(new Vector4(1, 1, 1, 1), "Size");
    ImGui.Separator();
    ImGui.TextColored(new Vector4(0.8f, 0.8f, 0.8f, 1), "Width:");
    ImGui.SameLine(90);
    ImGui.InputFloat("##positionwidth", ref width, 8, 16, "%.2f");
    ImGui.TextColored(new Vector4(0.8f, 0.8f, 0.8f, 1), "Height:");
    ImGui.SameLine(90);
    ImGui.InputFloat("##positionheight", ref height, 8, 16, "%.2f");

    return new Rectangle(x, y, width, height);
  }
}