using Hmk.Engine.Graphics;
using Hmk.Engine.Resources;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Systems.Inventory;

public class Item : Resource
{
  [Save]
  public string Name { get; set; } = string.Empty;

  [Save]
  public string Description { get; set; } = string.Empty;

  [Save]
  public Sprite? Sprite { get; set; } = null;

  public IUsable? UsableBehavior { get; set; } = null;

  public virtual void Use()
  {
    ArgumentNullException.ThrowIfNull(UsableBehavior, nameof(UsableBehavior));
    UsableBehavior.Use();
  }
}
