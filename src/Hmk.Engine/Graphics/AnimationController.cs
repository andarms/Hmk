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
  private string? currentAnimationName;

  public void PlayAnimation(string animationName)
  {

    if (currentAnimationName == animationName) return;
    currentAnimationName = animationName;
    if (Animations.TryGetValue(currentAnimationName, out var animation))
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
