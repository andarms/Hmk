namespace Hmk.Engine.Core;

public static class Settings
{
  public static int WindowWidth { get; internal set; } = 1280;
  public static int WindowHeight { get; internal set; } = 720;
  public static string WindowTitle { get; internal set; } = "Untitled Game";

  public static int TargetFPS { get; internal set; } = 60;

  public static void SetWindowSize(int width, int height)
  {
    WindowWidth = width;
    WindowHeight = height;
  }
}