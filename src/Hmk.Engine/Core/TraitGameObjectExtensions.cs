namespace Hmk.Engine.Core;

public static class TraitGameObjectExtensions
{
  public static void AddTrait<T>(this GameObject gameObject, T trait) where T : Trait
  {
    gameObject.Traits.Add(trait.GetType(), trait);
  }
  public static bool HasTrait<T>(this GameObject gameObject) where T : Trait
  {
    return gameObject.Traits.ContainsKey(typeof(T));
  }

  public static T? Trait<T>(this GameObject gameObject) where T : Trait
  {
    gameObject.Traits.TryGetValue(typeof(T), out var trait);
    return trait as T;
  }

  public static void RemoveTrait<T>(this GameObject gameObject) where T : Trait
  {
    gameObject.Traits.Remove(typeof(T));
  }
}
