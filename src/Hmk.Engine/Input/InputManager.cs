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

  /// <summary>
  /// Clear all actions.
  /// </summary>
  public static void Clear()
  {
    _actions.Clear();
  }
}
