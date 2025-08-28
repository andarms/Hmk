namespace Hmk.Engine.Core;

public static class ComponentGameObjectExtensions
{
  public static void AddComponent<T>(this GameObject gameObject, T component) where T : Component
  {
    // Upsert to avoid duplicate-key exceptions if the same component type is added twice
    gameObject.Components[component.GetType()] = component;
  }

  public static bool HasComponent<T>(this GameObject gameObject) where T : Component
  {
    return gameObject.Components.ContainsKey(typeof(T));
  }

  public static T? GetComponent<T>(this GameObject gameObject) where T : Component
  {
    gameObject.Components.TryGetValue(typeof(T), out var component);
    return component as T;
  }

  public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
  {
    gameObject.Components.Remove(typeof(T));
  }

  // Legacy support - these methods will continue to work with components
  [Obsolete("Use AddComponent instead")]
  public static void AddTrait<T>(this GameObject gameObject, T trait) where T : Component
  {
    gameObject.AddComponent(trait);
  }

  [Obsolete("Use HasComponent instead")]
  public static bool HasTrait<T>(this GameObject gameObject) where T : Component
  {
    return gameObject.HasComponent<T>();
  }

  [Obsolete("Use GetComponent instead")]
  public static T? Trait<T>(this GameObject gameObject) where T : Component
  {
    return gameObject.GetComponent<T>();
  }

  [Obsolete("Use RemoveComponent instead")]
  public static void RemoveTrait<T>(this GameObject gameObject) where T : Component
  {
    gameObject.RemoveComponent<T>();
  }
}
