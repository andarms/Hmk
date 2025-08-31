using System.Numerics;
using Hamaze.Engine.Core;

namespace Hamaze.Game.Scenes;

public class CameraSystem : WorldSystem
{
  Vector2 camera = new(0, 0);

  public override void Update(float DeltaTime, IEnumerable<IEntity> Entities)
  {
    Vector2 targetCamera = camera;

    if (IsKeyDown(KeyboardKey.Right)) targetCamera.X += 100 * DeltaTime;
    if (IsKeyDown(KeyboardKey.Left)) targetCamera.X -= 100 * DeltaTime;
    if (IsKeyDown(KeyboardKey.Up)) targetCamera.Y -= 100 * DeltaTime;
    if (IsKeyDown(KeyboardKey.Down)) targetCamera.Y += 100 * DeltaTime;

    // Smoothly interpolate camera towards targetCamera
    // float smoothness = 10f; // Higher is smoother
    // camera = Vector2.Lerp(camera, targetCamera, DeltaTime * smoothness);
    camera = targetCamera;

    Viewport.SetTarget(camera);
  }

  public override void Draw(IEnumerable<IEntity> _)
  {
    DrawCircleV(camera, 5, Color.Red);
  }
}
