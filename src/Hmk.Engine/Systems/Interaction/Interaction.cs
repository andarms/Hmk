using Hmk.Engine.Core;
using Hmk.Engine.Resources;

namespace Hmk.Engine.Systems.Interaction;

public abstract class Interaction : Resource
{
  protected const bool ShouldAlwaysExecute = true;
  public abstract bool CanPerformInteraction(GameObject actor);
  public abstract void Interact(GameObject actor);
}
