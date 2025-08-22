namespace Hmk.Engine.Debugging;

public static class DebugManager
{
  public static bool Active { get; private set; } = false;

  public static void Update(float dt)
  {
    if (IsKeyPressed(KeyboardKey.F1))
    {
      Active = !Active;
    }
  }
}