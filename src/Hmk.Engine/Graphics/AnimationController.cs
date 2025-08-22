using Hmk.Engine.Core;
using System.Linq;
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
      // Ensure the SpriteRenderer uses the same SpriteSheet instance as the animation
      var renderer = FindRenderer();
      if (renderer != null)
      {
        renderer.Sprite = currentAnimation.SpriteSheet;
      }

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

  private SpriteRenderer? FindRenderer()
  {
    // Prefer a sibling SpriteRenderer on the same parent; fall back to a child
    var sibling = Parent?.Children.OfType<SpriteRenderer>().FirstOrDefault();
    if (sibling != null) return sibling as SpriteRenderer;
    return Children.OfType<SpriteRenderer>().FirstOrDefault();
  }
}
