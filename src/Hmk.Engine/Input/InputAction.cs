namespace Hmk.Engine.Input;

/// <summary>
/// Represents a single logical action (e.g., "jump") bound to one or more inputs.
/// </summary>
public class InputAction(string name)
{
  public string Name { get; } = name;

  private readonly HashSet<KeyboardKey> keys = [];
  private readonly HashSet<GamepadButton> gamepadButtons = [];
  private readonly HashSet<MouseButton> mouseButtons = [];

  /// <summary>
  /// Optional dead zone value for analog inputs (reserved for future use).
  /// </summary>
  public float DeadZone { get; private set; } = 0.2f;

  public void AddKey(KeyboardKey key) => keys.Add(key);
  public void AddGamepadButton(GamepadButton button) => gamepadButtons.Add(button);
  public void AddMouseButton(MouseButton button) => mouseButtons.Add(button);
  public void SetDeadZone(float deadZone) => DeadZone = deadZone;

  /// <summary>
  /// Returns true if any bound input is currently down.
  /// </summary>
  public bool IsDown(int gamepad = 0)
  {
    foreach (var k in keys)
    {
      if (IsKeyDown(k)) return true;
    }
    foreach (var mb in mouseButtons)
    {
      if (IsMouseButtonDown(mb)) return true;
    }
    if (IsGamepadAvailable(gamepad))
    {
      foreach (var gb in gamepadButtons)
      {
        if (IsGamepadButtonDown(gamepad, gb)) return true;
      }
    }
    return false;
  }

  /// <summary>
  /// Returns true if any bound input was pressed this frame.
  /// </summary>
  public bool IsPressed(int gamepad = 0)
  {
    foreach (var k in keys)
    {
      if (Raylib.IsKeyPressed(k)) return true;
    }
    foreach (var mb in mouseButtons)
    {
      if (Raylib.IsMouseButtonPressed(mb)) return true;
    }
    if (Raylib.IsGamepadAvailable(gamepad))
    {
      foreach (var gb in gamepadButtons)
      {
        if (Raylib.IsGamepadButtonPressed(gamepad, gb)) return true;
      }
    }
    return false;
  }

  /// <summary>
  /// Returns true if any bound input was released this frame.
  /// </summary>
  public bool IsReleased(int gamepad = 0)
  {
    foreach (var k in keys)
    {
      if (IsKeyReleased(k)) return true;
    }
    foreach (var mb in mouseButtons)
    {
      if (IsMouseButtonReleased(mb)) return true;
    }
    if (IsGamepadAvailable(gamepad))
    {
      foreach (var gb in gamepadButtons)
      {
        if (IsGamepadButtonReleased(gamepad, gb)) return true;
      }
    }
    return false;
  }

  // Convenience aliases to make the distinction explicit.
  /// <summary>
  /// True while any bound input is held down (synonym for IsDown).
  /// </summary>
  public bool Pressed(int gamepad = 0) => IsDown(gamepad);

  /// <summary>
  /// Alias for held-state to avoid confusion with IsPressed (edge).
  /// </summary>
  public bool IsHeld(int gamepad = 0) => IsDown(gamepad);

  /// <summary>
  /// True only on the frame the input transitions from up to down (synonym for IsPressed).
  /// </summary>
  public bool JustPressed(int gamepad = 0) => IsPressed(gamepad);

  /// <summary>
  /// True only on the frame the input transitions from down to up (synonym for IsReleased).
  /// </summary>
  public bool JustReleased(int gamepad = 0) => IsReleased(gamepad);
}
