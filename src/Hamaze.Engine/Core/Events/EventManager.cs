namespace Hamaze.Engine.Core.Events;

public interface IEvent { }

public static class EventManager
{
  private static readonly Dictionary<Type, List<Action<IEvent>>> eventListeners = [];

  public static void Subscribe<T>(Action<T> listener) where T : IEvent
  {
    var eventType = typeof(T);
    if (!eventListeners.TryGetValue(eventType, out List<Action<IEvent>>? value))
    {
      value = [];
      eventListeners[eventType] = value;
    }

    value.Add(e => listener((T)e));
  }

  public static void Unsubscribe<T>(Action<T> listener) where T : IEvent
  {
    var eventType = typeof(T);
    if (eventListeners.TryGetValue(eventType, out List<Action<IEvent>>? value))
    {
      value.RemoveAll(e => e == (Action<IEvent>)(e => listener((T)e)));
    }
  }

  public static void Emit<T>(T eventToPublish) where T : IEvent
  {
    var eventType = typeof(T);
    if (eventListeners.TryGetValue(eventType, out List<Action<IEvent>>? value))
    {
      foreach (var listener in value)
      {
        listener(eventToPublish);
      }
    }
  }
}