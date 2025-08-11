using System.Numerics;

namespace Hmk.Engine.Core;

public static class DirectionsExtensions
{
  public static Vector2 ToVector2(this Directions direction)
  {
    return direction switch
    {
      Directions.Up => new Vector2(0, -1),
      Directions.Down => new Vector2(0, 1),
      Directions.Left => new Vector2(-1, 0),
      Directions.Right => new Vector2(1, 0),
      _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };
  }

  public static Directions Inverse(this Directions direction)
  {
    return direction switch
    {
      Directions.Up => Directions.Down,
      Directions.Down => Directions.Up,
      Directions.Left => Directions.Right,
      Directions.Right => Directions.Left,
      _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };
  }
}