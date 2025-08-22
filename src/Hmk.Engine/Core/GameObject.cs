using System.Numerics;
using Hmk.Engine.Collision;
using Hmk.Engine.Debug;

namespace Hmk.Engine.Core;

public class GameObject
{
  public string Name { get; set; } = string.Empty;
  public Vector2 Position { get; set; }
  public Collider? Collider { get; set; } = null;
  public List<GameObject> Collisions { get; } = [];

  public GameObject? Parent { get; set; } = null;
  public List<GameObject> Children { get; } = [];
  public Vector2 GlobalPosition
  {
    get => this.GetGlobalPosition();
    set => this.SetGlobalPosition(value);
  }

  internal Dictionary<Type, Trait> Traits { get; } = [];

  public GameObject() { }

  public virtual void Initialize()
  {
    Children.ForEach(child => child.Initialize());
  }

  public virtual void Update(float dt)
  {
    Children.ForEach(child => child.Update(dt));
  }

  public virtual void Draw()
  {
    Children.ForEach(child => child.Draw());
    if (DebugManager.Active)
    {
      this.DebugCollider();
    }
  }

  public virtual void Terminate()
  {
    Children.ForEach(child => child.Terminate());
    Children.Clear();
    Traits.Clear();
    Parent = null;
    if (Collider != null)
    {
      Collider = null;
      CollisionsManager.RemoveObject(this);
    }
  }
}