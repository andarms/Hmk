using System.Numerics;
using Hamaze.Engine.Core;
using Hamaze.Game.Pong.Components;
using Hamaze.Game.Pong.Systems;

namespace Hamaze.Game.Pong;

public class PongGameScene : Scene
{
  public override void Initialize()
  {
    base.Initialize();
    Entity paddle1 = new();
    AIPaddle aIPaddle1 = new();
    paddle1.Position = new(32, Viewport.ScreenSize.Y / 2 - aIPaddle1.Size.Y / 2);
    paddle1.AddComponent(new PaddleSprite());
    // paddle1.AddSystem(new PaddleInputSystem(speed: 300));
    paddle1.AddComponent(aIPaddle1);
    paddle1.AddSystem(new AIPaddleSystem());
    paddle1.SetCollider(new Collider(Vector2.Zero, aIPaddle1.Size));
    world.AddChild(paddle1);


    Entity paddle2 = new();
    AIPaddle aIPaddle2 = new();
    paddle2.Position = new(Viewport.ScreenSize.X - 32 - aIPaddle2.Size.X, Viewport.ScreenSize.Y / 2 - aIPaddle2.Size.Y / 2);
    paddle2.AddComponent(new PaddleSprite());
    paddle2.AddComponent(aIPaddle2);
    paddle2.AddSystem(new AIPaddleSystem());
    paddle2.SetCollider(new Collider(Vector2.Zero, aIPaddle2.Size));
    world.AddChild(paddle2);


    Entity ball = new()
    {
      Position = Viewport.ScreenSize / 2
    };
    ball.AddComponent(new Ball());
    ball.AddComponent(new BallSprite());
    ball.AddSystem(new BallMovementSystem());
    ball.SetCollider(new Collider(new Vector2(-16), new Vector2(32)));
    world.AddChild(ball);


    world.AddSystem(new PaddleCollisionSystem());
    world.AddSystem(new UpdateAITargetSystem());
    world.AddSystem(new ScoringSystem());
    world.AddSystem(new ResetBallSystem());
  }

  public override void Draw()
  {
    ClearBackground(Color.Black);
    base.Draw();
  }
}