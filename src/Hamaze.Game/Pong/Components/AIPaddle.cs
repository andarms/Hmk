using System.Numerics;
using Hamaze.Engine.Core;

namespace Hamaze.Game.Pong.Components;

public class AIPaddle : Component
{
  public Vector2 TargetPosition;
  public float Speed = 400f;
  public Vector2 Size = new(32, 128);
}
