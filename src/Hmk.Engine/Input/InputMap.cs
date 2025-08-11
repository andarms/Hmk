namespace Hmk.Engine.Input;

/// <summary>
/// Helper class for configuring input maps with fluent API
/// </summary>
public class InputMap
{
  private readonly Dictionary<string, InputAction> actions = [];

  /// <summary>
  /// Create a new input action and add it to the map
  /// </summary>
  public InputMap AddAction(string actionName)
  {
    actions[actionName] = new InputAction(actionName);
    return this;
  }

  /// <summary>
  /// Add a key binding to the last created action
  /// </summary>
  public InputMap WithKey(KeyboardKey key)
  {
    if (actions.Count > 0)
    {
      var lastAction = GetLastAction();
      lastAction?.AddKey(key);
    }
    return this;
  }

  /// <summary>
  /// Add multiple key bindings to the last created action
  /// </summary>
  public InputMap WithKeys(params KeyboardKey[] keys)
  {
    if (actions.Count > 0)
    {
      var lastAction = GetLastAction();
      if (lastAction != null)
      {
        foreach (var key in keys)
        {
          lastAction.AddKey(key);
        }
      }
    }
    return this;
  }

  /// <summary>
  /// Add a gamepad button binding to the last created action
  /// </summary>
  public InputMap WithGamepadButton(GamepadButton button)
  {
    if (actions.Count > 0)
    {
      var lastAction = GetLastAction();
      lastAction?.AddGamepadButton(button);
    }
    return this;
  }

  /// <summary>
  /// Add multiple gamepad button bindings to the last created action
  /// </summary>
  public InputMap WithGamepadButtons(params GamepadButton[] buttons)
  {
    if (actions.Count > 0)
    {
      var lastAction = GetLastAction();
      if (lastAction != null)
      {
        foreach (var button in buttons)
        {
          lastAction.AddGamepadButton(button);
        }
      }
    }
    return this;
  }

  /// <summary>
  /// Add a mouse button binding to the last created action
  /// </summary>
  public InputMap WithMouseButton(MouseButton button)
  {
    if (actions.Count > 0)
    {
      var lastAction = GetLastAction();
      lastAction?.AddMouseButton(button);
    }
    return this;
  }

  /// <summary>
  /// Add multiple mouse button bindings to the last created action
  /// </summary>
  public InputMap WithMouseButtons(params MouseButton[] buttons)
  {
    if (actions.Count > 0)
    {
      var lastAction = GetLastAction();
      if (lastAction != null)
      {
        foreach (var button in buttons)
        {
          lastAction.AddMouseButton(button);
        }
      }
    }
    return this;
  }

  /// <summary>
  /// Set dead zone for the last created action
  /// </summary>
  public InputMap WithDeadZone(float deadZone)
  {
    if (actions.Count > 0)
    {
      var lastAction = GetLastAction();
      lastAction?.SetDeadZone(deadZone);
    }
    return this;
  }

  /// <summary>
  /// Apply this input map to the InputService
  /// </summary>
  public void Apply()
  {
    foreach (var action in actions.Values)
    {
      InputManager.AddAction(action);
    }
  }

  /// <summary>
  /// Create a default input map with common game actions
  /// </summary>
  public static InputMap CreateDefault()
  {
    return new InputMap()
        // Movement actions
        .AddAction("up")
            .WithKeys(KeyboardKey.W, KeyboardKey.Up)
            .WithGamepadButton(GamepadButton.LeftFaceUp)
        .AddAction("down")
            .WithKeys(KeyboardKey.S, KeyboardKey.Down)
            .WithGamepadButton(GamepadButton.LeftFaceDown)
        .AddAction("left")
            .WithKeys(KeyboardKey.A, KeyboardKey.Left)
            .WithGamepadButton(GamepadButton.LeftFaceLeft)
        .AddAction("right")
            .WithKeys(KeyboardKey.D, KeyboardKey.Right)
            .WithGamepadButton(GamepadButton.LeftFaceRight)

        // Action buttons
        .AddAction("jump")
            .WithKey(KeyboardKey.Space)
            .WithGamepadButton(GamepadButton.RightFaceDown)
        .AddAction("attack")
            .WithKey(KeyboardKey.Enter)
            .WithMouseButton(MouseButton.Left)
            .WithGamepadButton(GamepadButton.RightFaceLeft)
        .AddAction("use")
            .WithKey(KeyboardKey.E)
            .WithGamepadButton(GamepadButton.RightFaceUp)
        .AddAction("run")
            .WithKey(KeyboardKey.LeftShift)
            .WithGamepadButton(GamepadButton.RightFaceRight)

        // Menu actions
        .AddAction("menu")
            .WithKey(KeyboardKey.Escape)
            .WithGamepadButton(GamepadButton.MiddleRight)
        .AddAction("pause")
            .WithKey(KeyboardKey.P)
            .WithGamepadButton(GamepadButton.MiddleLeft)
        .AddAction("confirm")
            .WithKey(KeyboardKey.Enter)
            .WithKey(KeyboardKey.Space)
            .WithGamepadButton(GamepadButton.RightFaceDown)
        .AddAction("cancel")
            .WithKey(KeyboardKey.Escape)
            .WithGamepadButton(GamepadButton.RightFaceRight);
  }

  private InputAction? GetLastAction()
  {
    if (actions.Count == 0) return null;

    InputAction? lastAction = null;
    foreach (var action in actions.Values)
    {
      lastAction = action;
    }
    return lastAction;
  }
}
