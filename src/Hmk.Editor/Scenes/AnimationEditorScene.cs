using System.Numerics;
using Hmk.Engine.Graphics;
using Hmk.Engine.Resources;
using Hmk.Engine.Scenes;
using Hmk.Engine.Serializer;
using IconFonts;
using ImGuiNET;
using rlImGui_cs;

namespace Hmk.Editor.Scenes;

public class AnimationEditorScene : Scene
{
  private AnimationController? animationController;
  private Texture2D? texture;
  private Vector2 position;
  private string? selectedAnimationKey;
  private Animation? selectedAnimation;
  private bool isPlaying = false;
  private string newAnimationName = "";
  private bool showAddAnimationPopup = false;

  // Animation preview variables
  private Vector2 animationPreviewPos = new(320, 0);
  private float animationPreviewScale = 2f;
  private int currentPreviewFrame = 0;
  private float previewTimer = 0f;

  public AnimationEditorScene(AnimationController controller)
  {
    animationController = controller;
  }

  public override void Initialize()
  {
    base.Initialize();
    rlImGui.Setup(true);
    Viewport.SetZoom(3);

    if (animationController?.Animations.Count > 0)
    {
      selectedAnimationKey = animationController.Animations.Keys.First();
      selectedAnimation = animationController.Animations[selectedAnimationKey];
      LoadTexture();
    }
  }

  private void LoadTexture()
  {
    if (selectedAnimation?.SpriteSheet?.Value != null)
    {
      texture = ResourcesManager.Textures[selectedAnimation.SpriteSheet.Value.TextureName];
      ResetPosition();
    }
  }

  private void ResetPosition()
  {
    if (texture.HasValue)
    {
      float x = Viewport.X + Viewport.GetSize().X / 2 - texture.Value.Width / 2;
      float y = Viewport.Y + Viewport.GetSize().Y / 2 - texture.Value.Height / 2;
      position = new Vector2(x, y);
    }
  }

  public override void Update(float dt)
  {
    base.Update(dt);

    // Update animation preview
    if (isPlaying && selectedAnimation != null && selectedAnimation.Frames.Count > 0)
    {
      previewTimer += dt;
      if (previewTimer >= selectedAnimation.Speed / 1000f)
      {
        currentPreviewFrame = (currentPreviewFrame + 1) % selectedAnimation.Frames.Count;
        previewTimer = 0f;
      }
    }
  }

  public override void Draw()
  {
    ClearBackground(Color.Black);

    rlImGui.Begin();
    BeginMode2D(Viewport.Camera);

    base.Draw();

    if (texture.HasValue && selectedAnimation?.SpriteSheet?.Value != null)
    {
      var spriteSheet = selectedAnimation.SpriteSheet.Value;

      // Draw background
      DrawRectangleV(position, new Vector2(texture.Value.Width, texture.Value.Height), Color.Magenta);

      // Draw full texture
      DrawTexturePro(
        texture.Value,
        new Rectangle(0, 0, texture.Value.Width, texture.Value.Height),
        new Rectangle(position.X, position.Y, texture.Value.Width, texture.Value.Height),
        Vector2.Zero, 0, Color.White);

      // Draw sprite sheet grid
      DrawSpriteSheetGrid(spriteSheet);

      // Highlight current frame
      if (selectedAnimation.Frames.Count > 0)
      {
        int currentFrameIndex = isPlaying ? currentPreviewFrame : 0;
        if (currentFrameIndex < selectedAnimation.Frames.Count)
        {
          int frameNumber = selectedAnimation.Frames[currentFrameIndex];
          DrawFrameHighlight(spriteSheet, frameNumber, Color.Yellow);
        }
      }

      // Draw animation preview
      DrawAnimationPreview(spriteSheet);
    }

    DrawInspector();
    EndMode2D();
    rlImGui.End();
  }

  private void DrawSpriteSheetGrid(SpriteSheet spriteSheet)
  {
    if (texture == null) return;

    int linesX = Math.Max(1, texture.Value.Width / spriteSheet.FrameWidth);
    int linesY = Math.Max(1, texture.Value.Height / spriteSheet.FrameHeight);

    float cellWidth = texture.Value.Width / (float)linesX;
    float cellHeight = texture.Value.Height / (float)linesY;

    // Draw vertical lines
    for (int x = 0; x <= linesX; x++)
    {
      float xPos = position.X + x * cellWidth;
      DrawLineV(new Vector2(xPos, position.Y), new Vector2(xPos, position.Y + texture.Value.Height), Color.Green);
    }

    // Draw horizontal lines
    for (int y = 0; y <= linesY; y++)
    {
      float yPos = position.Y + y * cellHeight;
      DrawLineV(new Vector2(position.X, yPos), new Vector2(position.X + texture.Value.Width, yPos), Color.Green);
    }
  }

  private void DrawFrameHighlight(SpriteSheet spriteSheet, int frameNumber, Color color)
  {
    if (texture == null) return;

    int linesX = Math.Max(1, texture.Value.Width / spriteSheet.FrameWidth);

    int col = frameNumber % linesX;
    int row = frameNumber / linesX;

    float cellWidth = texture.Value.Width / (float)linesX;
    float cellHeight = spriteSheet.FrameHeight;

    Rectangle frameRect = new Rectangle(
      position.X + col * cellWidth,
      position.Y + row * cellHeight,
      cellWidth,
      cellHeight
    );

    DrawRectangleLinesEx(frameRect, 2, color);
    DrawRectangleRec(frameRect, Fade(color, 0.25f));
  }

  private void DrawAnimationPreview(SpriteSheet spriteSheet)
  {
    if (selectedAnimation == null || selectedAnimation.Frames.Count == 0 || texture == null) return;

    // Get current frame to display
    int frameToShow = isPlaying ? currentPreviewFrame : 0;
    if (frameToShow >= selectedAnimation.Frames.Count) frameToShow = 0;

    int frameNumber = selectedAnimation.Frames[frameToShow];

    // Calculate frame position in sprite sheet
    int linesX = Math.Max(1, texture.Value.Width / spriteSheet.FrameWidth);
    int col = frameNumber % linesX;
    int row = frameNumber / linesX;

    // Source rectangle for the frame
    Rectangle sourceRect = new Rectangle(
      col * spriteSheet.FrameWidth,
      row * spriteSheet.FrameHeight,
      spriteSheet.FrameWidth,
      spriteSheet.FrameHeight
    );

    // Destination rectangle for preview (scaled up)
    Rectangle destRect = new Rectangle(
      animationPreviewPos.X,
      animationPreviewPos.Y,
      spriteSheet.FrameWidth * animationPreviewScale,
      spriteSheet.FrameHeight * animationPreviewScale
    );

    // Draw preview background
    DrawRectangleRec(destRect, Color.DarkGray);
    DrawRectangleLinesEx(destRect, 2, Color.White);

    // Draw the frame
    DrawTexturePro(texture.Value, sourceRect, destRect, Vector2.Zero, 0, Color.White);

    // Draw label
    DrawText("Animation Preview", (int)animationPreviewPos.X, (int)animationPreviewPos.Y - 25, 16, Color.White);

    // Draw frame info
    string frameInfo = $"Frame {frameToShow + 1}/{selectedAnimation.Frames.Count} (#{frameNumber})";
    DrawText(frameInfo, (int)animationPreviewPos.X, (int)(animationPreviewPos.Y + destRect.Height + 5), 12, Color.LightGray);

    // Draw animation status
    string status = isPlaying ? "Playing" : "Stopped";
    DrawText(status, (int)animationPreviewPos.X, (int)(animationPreviewPos.Y + destRect.Height + 20), 12, isPlaying ? Color.Green : Color.Red);
  }

  private void DrawInspector()
  {
    ImGui.SetNextWindowPos(new(0, 0));
    ImGui.SetNextWindowSize(new(300, Viewport.Height));
    ImGui.Begin("#Inspector", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar);

    ImGui.TextColored(new Vector4(1, 1, 1, 1), "Animation Editor");
    ImGui.SameLine(270);
    if (ImGui.Button($"{FontAwesome6.Xmark}", new(20)))
    {
      SceneManager.PopScene();
    }

    ImGui.Spacing();
    ImGui.Separator();
    ImGui.Spacing();

    if (animationController != null)
    {
      DrawAnimationsList();
      ImGui.Spacing();
      DrawAnimationControls();
      ImGui.Spacing();
      DrawAnimationProperties();
      ImGui.Spacing();
      DrawFramesList();
    }

    ImGui.Spacing();
    ImGui.Spacing();
    if (ImGui.Button($"{FontAwesome6.FloppyDisk} Save", new(280, 20)))
    {
      if (animationController != null)
      {
        Console.WriteLine(animationController.Serialize());
      }
    }

    DrawAddAnimationPopup();
    ImGui.End();
  }

  private void DrawAnimationsList()
  {
    ImGui.TextColored(new Vector4(1, 1, 1, 1), "Animations");
    ImGui.Separator();

    if (ImGui.Button($"{FontAwesome6.Plus} Add Animation", new(280, 20)))
    {
      showAddAnimationPopup = true;
      ImGui.OpenPopup("AddAnimationPopup");
    }

    ImGui.Spacing();

    if (animationController?.Animations != null)
    {
      foreach (var kvp in animationController.Animations)
      {
        bool isSelected = selectedAnimationKey == kvp.Key;
        if (ImGui.Selectable(kvp.Key, isSelected))
        {
          selectedAnimationKey = kvp.Key;
          selectedAnimation = kvp.Value;
          LoadTexture();
          isPlaying = false;
          currentPreviewFrame = 0;
          previewTimer = 0f;
        }

        if (isSelected)
        {
          ImGui.SameLine(250);
          if (ImGui.Button($"{FontAwesome6.Trash}##{kvp.Key}", new(20)))
          {
            animationController.Animations.Remove(kvp.Key);
            if (selectedAnimationKey == kvp.Key)
            {
              selectedAnimationKey = animationController.Animations.Keys.FirstOrDefault();
              selectedAnimation = selectedAnimationKey != null ? animationController.Animations[selectedAnimationKey] : null;
              LoadTexture();
            }
          }
        }
      }
    }
  }

  private void DrawAnimationControls()
  {
    if (selectedAnimation == null) return;

    ImGui.TextColored(new Vector4(1, 1, 1, 1), "Controls");
    ImGui.Separator();

    if (ImGui.Button(isPlaying ? $"{FontAwesome6.Pause} Pause" : $"{FontAwesome6.Play} Play", new(135, 20)))
    {
      isPlaying = !isPlaying;
      if (isPlaying)
      {
        currentPreviewFrame = 0;
        previewTimer = 0f;
      }
    }

    ImGui.SameLine();
    if (ImGui.Button($"{FontAwesome6.Stop} Stop", new(135, 20)))
    {
      isPlaying = false;
      currentPreviewFrame = 0;
      previewTimer = 0f;
    }
  }

  private void DrawAnimationProperties()
  {
    if (selectedAnimation == null) return;

    ImGui.TextColored(new Vector4(1, 1, 1, 1), "Properties");
    ImGui.Separator();

    // Speed
    float speed = selectedAnimation.Speed;
    ImGui.TextColored(new Vector4(0.8f, 0.8f, 0.8f, 1), "Speed (ms):");
    ImGui.SameLine(100);
    ImGui.PushItemWidth(170);
    if (ImGui.InputFloat("##speed", ref speed, 10, 50, "%.1f"))
    {
      selectedAnimation.Speed = Math.Max(1, speed);
    }
    ImGui.PopItemWidth();

    // SpriteSheet
    ImGui.TextColored(new Vector4(0.8f, 0.8f, 0.8f, 1), "SpriteSheet:");
    ImGui.PushItemWidth(270);
    string currentSpriteSheet = selectedAnimation.SpriteSheet?.Path ?? "";
    if (ImGui.BeginCombo("##spritesheet", currentSpriteSheet))
    {
      // Filter resources that are SpriteSheet types
      foreach (var resource in ResourcesManager.Resources.Where(r => r.Value is SpriteSheet))
      {
        if (ImGui.Selectable(resource.Key))
        {
          selectedAnimation.SpriteSheet = new ResourceReference<SpriteSheet> { Path = resource.Key };
          LoadTexture();
        }
      }
      ImGui.EndCombo();
    }
    ImGui.PopItemWidth();
  }

  private void DrawFramesList()
  {
    if (selectedAnimation == null) return;

    ImGui.TextColored(new Vector4(1, 1, 1, 1), "Frames");
    ImGui.Separator();

    if (ImGui.Button($"{FontAwesome6.Plus} Add Frame", new(135, 20)))
    {
      selectedAnimation.Frames.Add(0);
    }

    ImGui.SameLine();
    if (ImGui.Button($"{FontAwesome6.Trash} Clear All", new(135, 20)))
    {
      selectedAnimation.Frames.Clear();
    }

    ImGui.Spacing();

    for (int i = 0; i < selectedAnimation.Frames.Count; i++)
    {
      ImGui.PushID(i);

      int frame = selectedAnimation.Frames[i];
      ImGui.TextColored(new Vector4(0.8f, 0.8f, 0.8f, 1), $"Frame {i}:");
      ImGui.SameLine(80);
      ImGui.PushItemWidth(150);
      if (ImGui.InputInt("##frame", ref frame, 1, 1))
      {
        selectedAnimation.Frames[i] = Math.Max(0, frame);
      }
      ImGui.PopItemWidth();

      ImGui.SameLine();
      if (ImGui.Button($"{FontAwesome6.Trash}", new(20)))
      {
        selectedAnimation.Frames.RemoveAt(i);
        i--; // Adjust index since we removed an item
      }

      if (i < selectedAnimation.Frames.Count - 1)
      {
        ImGui.SameLine();
        if (ImGui.Button($"{FontAwesome6.ArrowDown}", new(20)))
        {
          (selectedAnimation.Frames[i], selectedAnimation.Frames[i + 1]) = (selectedAnimation.Frames[i + 1], selectedAnimation.Frames[i]);
        }
      }

      if (i > 0)
      {
        ImGui.SameLine();
        if (ImGui.Button($"{FontAwesome6.ArrowUp}", new(20)))
        {
          (selectedAnimation.Frames[i], selectedAnimation.Frames[i - 1]) = (selectedAnimation.Frames[i - 1], selectedAnimation.Frames[i]);
        }
      }

      ImGui.PopID();
    }
  }

  private void DrawAddAnimationPopup()
  {
    if (showAddAnimationPopup && ImGui.BeginPopupModal("AddAnimationPopup", ImGuiWindowFlags.AlwaysAutoResize))
    {
      ImGui.Text("Animation Name:");
      ImGui.InputText("##animname", ref newAnimationName, 50);

      if (ImGui.Button("Add", new(100, 20)))
      {
        if (!string.IsNullOrWhiteSpace(newAnimationName) &&
            animationController != null &&
            !animationController.Animations.ContainsKey(newAnimationName))
        {
          var newAnimation = new Animation
          {
            Speed = 180,
            Frames = [0]
          };

          if (selectedAnimation?.SpriteSheet != null)
          {
            newAnimation.SpriteSheet = selectedAnimation.SpriteSheet;
          }

          animationController.Animations[newAnimationName] = newAnimation;
          selectedAnimationKey = newAnimationName;
          selectedAnimation = newAnimation;
          LoadTexture();
        }
        showAddAnimationPopup = false;
        newAnimationName = "";
        ImGui.CloseCurrentPopup();
      }

      ImGui.SameLine();
      if (ImGui.Button("Cancel", new(100, 20)))
      {
        showAddAnimationPopup = false;
        newAnimationName = "";
        ImGui.CloseCurrentPopup();
      }

      ImGui.EndPopup();
    }
  }
}