using System;

namespace Hmk.Engine.Core;

public class Signal : IDisposable
{
  private Action? _action;

  public void Connect(Action action)
  {
    _action += action;
  }

  public void Disconnect(Action action)
  {
    _action -= action;
  }

  public void Emit()
  {
    _action?.Invoke();
  }

  public void DisconnectAll()
  {
    _action = null;
  }

  public void Dispose() => DisconnectAll();
}

// Generic Signal class for single parameter
public class Signal<T> : IDisposable
{
  private Action<T>? _action;

  public void Connect(Action<T> action)
  {
    _action += action;
  }

  public void Disconnect(Action<T> action)
  {
    _action -= action;
  }

  public void Emit(T arg)
  {
    _action?.Invoke(arg);
  }

  public void DisconnectAll()
  {
    _action = null;
  }

  public void Dispose() => DisconnectAll();
}

// Generic Signal class for two parameters
public class Signal<T1, T2> : IDisposable
{
  private Action<T1, T2>? _action;

  public void Connect(Action<T1, T2> action)
  {
    _action += action;
  }

  public void Disconnect(Action<T1, T2> action)
  {
    _action -= action;
  }

  public void Emit(T1 arg1, T2 arg2)
  {
    _action?.Invoke(arg1, arg2);
  }

  public void DisconnectAll()
  {
    _action = null;
  }

  public void Dispose()
  {
    DisconnectAll();
  }
}

// Generic Signal class for three parameters
public class Signal<T1, T2, T3> : IDisposable
{
  private Action<T1, T2, T3>? _action;

  public void Connect(Action<T1, T2, T3> action)
  {
    _action += action;
  }

  public void Disconnect(Action<T1, T2, T3> action)
  {
    _action -= action;
  }

  public void Emit(T1 arg1, T2 arg2, T3 arg3)
  {
    _action?.Invoke(arg1, arg2, arg3);
  }

  public void DisconnectAll()
  {
    _action = null;
  }

  public void Dispose()
  {
    DisconnectAll();
  }
}

// Generic Signal class for four parameters
public class Signal<T1, T2, T3, T4> : IDisposable
{
  private Action<T1, T2, T3, T4>? _action;

  public void Connect(Action<T1, T2, T3, T4> action)
  {
    _action += action;
  }

  public void Disconnect(Action<T1, T2, T3, T4> action)
  {
    _action -= action;
  }

  public void Emit(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
  {
    _action?.Invoke(arg1, arg2, arg3, arg4);
  }

  public void DisconnectAll()
  {
    _action = null;
  }

  public void Dispose()
  {
    DisconnectAll();
  }
}

