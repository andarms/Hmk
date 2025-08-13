using System.Numerics;

namespace Hmk.Engine.Input;

/// <summary>
/// Central input manager that holds actions and offers query helpers.
/// </summary>
public static class InputManager
{
  private static readonly Dictionary<string, InputAction> _actions = [];

  /// <summary>
  /// Add or replace an action.
  /// </summary>
  public static void AddAction(InputAction action)
  {
    _actions[action.Name] = action;
  }

  /// <summary>
  /// Remove an action by name.
  /// </summary>
  public static bool RemoveAction(string name) => _actions.Remove(name);

  /// <summary>
  /// Try to get an action by name.
  /// </summary>
  public static bool TryGet(string name, out InputAction? action) => _actions.TryGetValue(name, out action);

  /// <summary>
  /// Returns true if the named action exists and is down.
  /// </summary>
  public static bool IsDown(string action, int gamepad = 0)
  {
    return _actions.TryGetValue(action, out var a) && a.IsDown(gamepad);
  }

  /// <summary>
  /// Returns true if the named action exists and was pressed this frame.
  /// </summary>
  public static bool IsPressed(string action, int gamepad = 0)
  {
    return _actions.TryGetValue(action, out var a) && a.IsPressed(gamepad);
  }

  /// <summary>
  /// Returns true if the named action exists and was released this frame.
  /// </summary>
  public static bool IsReleased(string action, int gamepad = 0)
  {
    return _actions.TryGetValue(action, out var a) && a.IsReleased(gamepad);
  }

  // Aliases for clarity between held vs edge events
  public static bool Pressed(string action, int gamepad = 0)
    => IsDown(action, gamepad);

  public static bool IsHeld(string action, int gamepad = 0)
    => IsDown(action, gamepad);

  public static bool JustPressed(string action, int gamepad = 0)
    => IsPressed(action, gamepad);

  public static bool JustReleased(string action, int gamepad = 0)
    => IsReleased(action, gamepad);


  public static Vector2 GetVector(string negativeXAction, string positiveXAction, string negativeYAction, string positiveYAction, int gamepad = 0)
  {
    float x = 0;
    float y = 0;

    if (IsHeld(positiveXAction, gamepad))
      x += 1;
    if (IsHeld(negativeXAction, gamepad))
      x -= 1;
    if (IsHeld(positiveYAction, gamepad))
      y += 1;
    if (IsHeld(negativeYAction, gamepad))
      y -= 1;

    return new Vector2(x, y);
  }

  /// <summary>
  /// Get the current value of the gamepad left stick.
  /// </summary>
  public static Vector2 GetGamepadLeftStick(int gamepad = 0)
  {
    // Implementation for getting the gamepad left stick value
    return Vector2.Zero;
  }

  /// <summary>
  /// Clear all actions.
  /// </summary>
  public static void Clear()
  {
    _actions.Clear();
  }
}
