using Hmk.Engine.Core;
using Hmk.Engine.Resources;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Graphics;


public class Animation : GameObject
{
  [Save]
  public ResourceReference<SpriteSheet> SpriteSheet { get; set; } = null!;

  [Save]
  public List<int> Frames { get; set; } = [];

  private int currentFrame = 0;
  private float timer = 0f;
  public float Speed { get; set; } = 100f; // milliseconds per frame

  public override void Initialize()
  {
    base.Initialize();
    ArgumentNullException.ThrowIfNull(SpriteSheet, nameof(SpriteSheet));
    ArgumentNullException.ThrowIfNull(Frames, nameof(Frames));

    currentFrame = 0;
    SpriteSheet.Value.SetFrame(Frames[currentFrame]);
  }

  public override void Update(float dt)
  {
    base.Update(dt);
    timer += dt;

    if (timer >= Speed / 1000f)
    {
      currentFrame = (currentFrame + 1) % Frames.Count;
      SpriteSheet.Value.SetFrame(Frames[currentFrame]);
      timer = 0f;
    }
  }
}