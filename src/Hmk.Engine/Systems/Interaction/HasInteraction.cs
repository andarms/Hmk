using Hmk.Engine.Core;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Systems.Interaction;

public class HasInteraction : Trait
{
  [Save]
  public List<Interaction> Interactions { get; set; } = [];

  [Save]
  public Directions? InteractionSide { get; set; } = null;

  public void HandleInteraction(GameObject actor)
  {
    foreach (var interaction in Interactions)
    {
      if (interaction.CanPerformInteraction(actor))
      {
        interaction.Interact(actor);
      }
    }
  }

  public void AddInteraction(Interaction interaction)
  {
    Interactions.Add(interaction);
  }

  public void RemoveInteraction(Interaction interaction)
  {
    Interactions.Remove(interaction);
  }
}
