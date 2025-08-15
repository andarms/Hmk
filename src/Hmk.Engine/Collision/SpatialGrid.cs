using Hmk.Engine.Core;

namespace Hmk.Engine.Collision;

public class SpatialGrid(int cellSize, int width, int height)
{
  private readonly HashSet<GameObject>[,] grid = new HashSet<GameObject>[width / cellSize, height / cellSize];

  public void AddObject(GameObject obj)
  {
    if (obj.Collider == null) return;
    // Use global top-left of the collider (accounts for parent transforms and offset)
    var topLeft = obj.GlobalPosition + obj.Collider.Offset;
    int cellX = (int)(topLeft.X / cellSize);
    int cellY = (int)(topLeft.Y / cellSize);

    if (cellX < 0 || cellX >= grid.GetLength(0) || cellY < 0 || cellY >= grid.GetLength(1)) return;

    if (grid[cellX, cellY] == null)
      grid[cellX, cellY] = [];

    grid[cellX, cellY].Add(obj);
  }

  public void RemoveObject(GameObject obj)
  {
    var topLeft = (obj.Collider != null)
      ? obj.GlobalPosition + obj.Collider.Offset
      : obj.GlobalPosition;
    int cellX = (int)(topLeft.X / cellSize);
    int cellY = (int)(topLeft.Y / cellSize);

    if (cellX < 0 || cellX >= grid.GetLength(0) || cellY < 0 || cellY >= grid.GetLength(1)) return;

    grid[cellX, cellY]?.Remove(obj);
  }

  public void UpdateObject(GameObject obj)
  {
    RemoveObject(obj);
    AddObject(obj);
  }

  public IEnumerable<GameObject> GetPotentialCollisions(GameObject obj)
  {
    var topLeft = (obj.Collider != null)
      ? obj.GlobalPosition + obj.Collider.Offset
      : obj.GlobalPosition;
    int cellX = (int)(topLeft.X / cellSize);
    int cellY = (int)(topLeft.Y / cellSize);

    if (cellX < 0 || cellX >= grid.GetLength(0) || cellY < 0 || cellY >= grid.GetLength(1)) yield break;

    // check around neighbors
    for (int x = cellX - 1; x <= cellX + 1; x++)
    {
      for (int y = cellY - 1; y <= cellY + 1; y++)
      {
        if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1))
        {
          continue;
        }

        foreach (var nearby in grid[x, y] ?? [])
        {
          if (nearby != null && obj != nearby) yield return nearby;
        }
      }
    }
  }


  public void Clear()
  {
    for (int x = 0; x < grid.GetLength(0); x++)
    {
      for (int y = 0; y < grid.GetLength(1); y++)
      {
        grid[x, y]?.Clear();
      }
    }
  }
}