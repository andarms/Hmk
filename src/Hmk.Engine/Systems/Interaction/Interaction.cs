using Hmk.Engine.Core;

namespace Hmk.Engine.Systems.Interaction;

public abstract class Interaction : Component
{
  protected const bool ShouldAlwaysExecute = true;
  public abstract bool CanPerformInteraction(GameObject actor);
  public abstract void Interact(GameObject actor);
}
