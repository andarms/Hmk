using System.Numerics;

namespace Hmk.Engine.Core;

public static class GameObjectChildrenExtension
{
  public static void AddChild(this GameObject parent, GameObject child)
  {
    parent.Children.Add(child);
    child.Parent = parent;
  }

  public static void RemoveChild(this GameObject parent, GameObject child)
  {
    parent.Children.Remove(child);
    child.Parent = null;
  }


  public static IEnumerable<T> GetChildren<T>(this GameObject gameObject) where T : GameObject
  {
    return gameObject.Children.OfType<T>();
  }

  public static T? GetChild<T>(this GameObject gameObject) where T : GameObject
  {
    return gameObject.Children.OfType<T>().FirstOrDefault();
  }
}