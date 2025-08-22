using Hmk.Engine.Core;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Graphics;

public class AnimationController : GameObject
{
  [Save]
  public Dictionary<string, Animation> Animations { get; } = [];

  [Save]
  public string? Start { get; set; }

  private Animation? currentAnimation;

  public void PlayAnimation(string animationName)
  {
    if (Animations.TryGetValue(animationName, out var animation))
    {
      currentAnimation = animation;
      currentAnimation.Initialize();
    }
  }

  public override void Initialize()
  {
    base.Initialize();
    // Auto-play requested animation or the first available
    if (!string.IsNullOrWhiteSpace(Start) && Animations.ContainsKey(Start))
    {
      PlayAnimation(Start);
    }
    else if (currentAnimation == null && Animations.Count > 0)
    {
      var first = Animations.Keys.First();
      PlayAnimation(first);
    }
  }

  public override void Update(float dt)
  {
    currentAnimation?.Update(dt);
  }
}
