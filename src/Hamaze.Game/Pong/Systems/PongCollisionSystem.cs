using System.Numerics;
using Hamaze.Engine.Core;
using Hamaze.Game.Pong.Components;

namespace Hamaze.Game.Pong.Systems;

public class PongCollisionSystem : WorldSystem
{
  public override void Update(float dt, IEnumerable<IEntity> entities)
  {
    base.Update(dt, entities);
    var checkedPairs = new HashSet<(IEntity, IEntity)>();
    foreach (IEntity obj1 in entities)
    {
      foreach (IEntity obj2 in entities)
      {
        var pair = obj1.GetHashCode() < obj2.GetHashCode() ? (obj1, obj2) : (obj2, obj1);
        if (checkedPairs.Contains(pair)) { continue; }
        checkedPairs.Add(pair);
        if (obj1 == obj2) { continue; }

        if (CheckCollisionRecs(obj1.Bounds, obj2.Bounds))
        {
          var ball = obj1.GetComponent<Ball>() ?? obj2.GetComponent<Ball>();
          if (ball != null && !ball.Immune)
          {
            ball.Velocity *= -1;
            ball.SpeedIncrease += 0.1f;
            ball.SetImmunity(1f);
          }
        }
      }
    }
  }
}
