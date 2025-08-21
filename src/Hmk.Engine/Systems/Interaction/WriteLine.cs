using Hmk.Engine.Core;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Systems.Interaction;


public class WriteLine : Interaction
{
  [Save]
  public string Message { get; set; } = string.Empty;

  public override bool CanPerformInteraction(GameObject actor)
  {
    return ShouldAlwaysExecute;
  }

  public override void Interact(GameObject actor)
  {
    Console.WriteLine(Message);
  }
}