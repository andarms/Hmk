using Hmk.Engine.Collision;
using Hmk.Engine.Core;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Systems.Attack;

public class Hitbox : GameObject
{
  [Save]
  public IDamageCalculator DamageCalculator { get; set; } = new NoDamage();

  public override void Initialize()
  {
    base.Initialize();
    ArgumentNullException.ThrowIfNull(Collider, nameof(Collider));
    CollisionsManager.AddObject(this);
  }
}