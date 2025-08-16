using Hmk.Engine.Core;

namespace Hmk.Engine.Scenes;

public enum LayerPriority
{
  Background = -1,
  Default = 0,
  Foreground = 1,
  UI = 2
}

public class ObjectLayer(string name)
{

  public string Name { get; set; } = name;

  public readonly List<GameObject> Instances = [];

  public LayerPriority Priority { get; set; } = LayerPriority.Default;

  public virtual void Initialize()
  {
    Instances.ForEach(obj => obj.Initialize());
  }

  public virtual void Update(float dt)
  {
    Instances.ForEach(obj => obj.Update(dt));
  }

  public virtual void Draw()
  {

    var sortedInstances = Instances.OrderBy(i => i.GlobalPosition.Y);

    foreach (var obj in sortedInstances)
    {
      obj.Draw();
    }
  }

  public virtual void Dispose()
  {
    Instances.ForEach(obj => obj.Terminate());
    Instances.Clear();
  }
}