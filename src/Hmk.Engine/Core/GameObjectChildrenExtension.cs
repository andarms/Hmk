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

  public static Vector2 GetGlobalPosition(this GameObject gameObject)
  {
    if (gameObject.Parent != null)
    {
      return gameObject.Parent.GetGlobalPosition() + gameObject.Position;
    }
    return gameObject.Position;
  }


  public static IEnumerable<T> GetChildren<T>(this GameObject gameObject) where T : GameObject
  {
    return gameObject.Children.OfType<T>();
  }

  public static T? GetChild<T>(this GameObject gameObject) where T : GameObject
  {
    return gameObject.Children.OfType<T>().FirstOrDefault();
  }


  public static void SetGlobalPosition(this GameObject gameObject, Vector2 position)
  {
    if (gameObject.Parent != null)
    {
      gameObject.Position = position - gameObject.Parent.GetGlobalPosition();
    }
    else
    {
      gameObject.Position = position;
    }
  }
}