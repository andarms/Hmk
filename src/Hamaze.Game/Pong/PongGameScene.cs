using System.Numerics;
using Hamaze.Engine.Core;

namespace Hamaze.Game.Pong;


public class PaddleSprite : Component
{
  public Vector2 Size = new(20, 100);
  public override void Draw(IReadOnlyEntity entity)
  {
    base.Draw(entity);
    // Draw the paddle sprite
    DrawRectangleV(entity.GlobalPosition, Size, Color.White);
    DrawRectangleLinesEx(
      entity.Bounds,
      2,
      Color.Red
    );
  }
}


public class Ball : Component
{
  public Vector2 Velocity = new(1, 1);
  public float SpeedIncrease = 1f;
}

public class BallSprite : Component
{
  readonly int radius = 16;

  public override void Draw(IReadOnlyEntity entity)
  {
    base.Draw(entity);
    // Draw the ball sprite
    DrawCircleV(entity.GlobalPosition, radius, Color.White);
    DrawRectangleLinesEx(
      entity.Bounds,
      1,
      Color.Red
    );
  }
}


public class BallMovementSystem() : EntitySystem
{
  public float speed = 300f;
  public override void Update(float dt, IEntity entity)
  {
    base.Update(dt, entity);
    Ball ball = entity.Require<Ball>();
    entity.Position += ball.Velocity * ball.SpeedIncrease * speed * dt;
    if (entity.Position.X < 0 || entity.Position.X > Viewport.ScreenSize.X)
    {
      ball.Velocity.X *= -1;
      Console.WriteLine("Score!");
      ball.SpeedIncrease = 1f;
    }
    if (entity.Position.Y < 0 || entity.Position.Y > Viewport.ScreenSize.Y)
    {
      ball.Velocity.Y *= -1;
    }
  }
}

public class PaddleInputSystem(float speed) : EntitySystem
{
  Vector2 paddleSize;
  public override void Initialize(IEntity entity)
  {
    base.Initialize(entity);
    paddleSize = entity.Require<PaddleSprite>().Size;
  }

  public override void Update(float dt, IEntity entity)
  {
    base.Update(dt, entity);


    if (IsKeyDown(KeyboardKey.Up))
    {
      entity.Position = new(entity.Position.X, entity.Position.Y - speed * dt);
    }
    if (IsKeyDown(KeyboardKey.Down))
    {
      entity.Position = new(entity.Position.X, entity.Position.Y + speed * dt);
    }
    entity.Position = Vector2.Clamp(entity.Position, new Vector2(0, 0), Viewport.ScreenSize - paddleSize);
  }
}


public class PaddleCollisionSystem : WorldSystem
{
  public override void Update(float dt, IEnumerable<IEntity> entities)
  {
    base.Update(dt, entities);
    var checkedPairs = new HashSet<(IEntity, IEntity)>();
    foreach (IEntity obj1 in entities)
    {
      foreach (IEntity obj2 in entities)
      {
        var pair = obj1.GetHashCode() < obj2.GetHashCode() ? (obj1, obj2) : (obj2, obj1);
        if (checkedPairs.Contains(pair)) { continue; }
        checkedPairs.Add(pair);
        if (obj1 == obj2) { continue; }

        if (CheckCollisionRecs(obj1.Bounds, obj2.Bounds))
        {
          var ball = obj1.GetComponent<Ball>() ?? obj2.GetComponent<Ball>();
          if (ball != null)
          {
            ball.Velocity *= -1;
            ball.SpeedIncrease += 0.1f;
          }
        }
      }
    }
  }
}


public class AIPaddle : Component
{
  public Vector2 TargetPosition;
  public float Speed = 400f;
}

public class AIPaddleSystem : EntitySystem
{
  public override void Update(float dt, IEntity entity)
  {
    base.Update(dt, entity);
    AIPaddle ai = entity.Require<AIPaddle>();
    Vector2 direction = Vector2.Normalize(ai.TargetPosition - entity.Position);
    entity.Position = new Vector2(entity.Position.X, entity.Position.Y + direction.Y * ai.Speed * dt);
  }
}

public class UpdateAITargetSystem : WorldSystem
{
  public override void Update(float dt, IEnumerable<IEntity> entities)
  {
    base.Update(dt, entities);
    var ball = entities.FirstOrDefault(e => e.HasComponent<Ball>());
    var aiPaddle = entities.FirstOrDefault(e => e.HasComponent<AIPaddle>());

    if (ball != null && aiPaddle != null)
    {
      if (ball.Position.X < Viewport.ScreenSize.X / 2) return; // Only track when ball is on AI side
      AIPaddle ai = aiPaddle.Require<AIPaddle>();
      ai.TargetPosition = ball.Position;
    }
  }
}

public class PongGameScene : Scene
{
  public override void Initialize()
  {
    base.Initialize();
    Entity paddle1 = new();
    PaddleSprite sprite = new();
    paddle1.Position = new(32, Viewport.ScreenSize.Y / 2 - sprite.Size.Y / 2);
    paddle1.AddComponent(sprite);
    paddle1.AddSystem(new PaddleInputSystem(speed: 300));
    paddle1.SetCollider(new Collider(Vector2.Zero, sprite.Size));
    world.AddChild(paddle1);


    Entity paddle2 = new();
    PaddleSprite sprite2 = new();
    paddle2.Position = new(Viewport.ScreenSize.X - 32 - sprite2.Size.X, Viewport.ScreenSize.Y / 2 - sprite2.Size.Y / 2);
    paddle2.AddComponent(sprite2);
    paddle2.AddComponent(new AIPaddle());
    paddle2.AddSystem(new AIPaddleSystem());
    paddle2.SetCollider(new Collider(Vector2.Zero, sprite2.Size));
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
  }

  public override void Draw()
  {
    ClearBackground(Color.Black);
    base.Draw();
  }
}