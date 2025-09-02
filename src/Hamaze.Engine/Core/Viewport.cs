using System.Numerics;

namespace Hamaze.Engine.Core;

public static class Viewport
{
  public static int X { get; internal set; } = 0;
  public static int Y { get; internal set; } = 0;
  public static int Width { get; internal set; } = 1280;
  public static int Height { get; internal set; } = 720;

  public static int HalfWidth => Width / 2;
  public static int HalfHeight => Height / 2;
  public static float AspectRatio => (float)Width / Height;
  public static float Zoom { get; set; } = 3.0f;


  public static Vector2 ScreenSize => new(Width, Height);

  public static Camera2D Camera { get; private set; } = new Camera2D
  {
    Rotation = 0.0f,
    Zoom = Zoom,
    Offset = new Vector2(HalfWidth, HalfHeight)
  };

  public static void SetViewport(int x, int y, int width, int height)
  {
    X = x;
    Y = y;
    Width = width;
    Height = height;
  }



  public static void SetZoom(float zoom)
  {
    Zoom = zoom;
    Camera = new Camera2D
    {
      Rotation = 0.0f,
      Zoom = Zoom,
      Offset = Camera.Offset
    };
  }


  public static Vector2 GetSize()
  {
    return new Vector2(Width / Zoom, Height / Zoom);
  }

  public static void Target(Vector2 position)
  {
    throw new NotImplementedException();
  }

  public static void SetTarget(Vector2 position)
  {
    Camera = new Camera2D
    {
      Target = position,
      Rotation = 0.0f,
      Zoom = Zoom,
      Offset = Camera.Offset
    };
  }
}