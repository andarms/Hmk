using Hmk.Engine.Core;

namespace Hmk.Engine.Scenes;

public class Scene
{
  protected readonly Dictionary<string, ObjectLayer> layers = [];
  protected readonly List<string> sortedLayers = [];
  public const string DefaultLayerName = "Default";
  public const string BackgroundLayerName = "Background";
  public const string ForegroundLayerName = "Foreground";
  public const string UILayer = "UIOverlay";


  public virtual Color BackgroundColor { get; } = Color.Black;

  public Camera2D Camera { get; } = new()
  {
    Zoom = 3.0f,
  };


  public bool IsActive { get; protected set; } = true;
  public bool IsPaused { get; protected set; } = false;

  public Scene()
  {
    ObjectLayer defaultLayer = new(DefaultLayerName);
    layers.Add(defaultLayer.Name, defaultLayer);
    sortedLayers.Add(defaultLayer.Name);

    layers.Add(UILayer, new ObjectLayer(UILayer)
    {
      Priority = LayerPriority.UI
    });
    sortedLayers.Add(UILayer);
  }

  #region Child Management
  public void AddChild(GameObject child, string layerName = DefaultLayerName)
  {
    if (!layers.TryGetValue(layerName, out ObjectLayer? layer))
    {
      layer = new ObjectLayer(layerName);
      layers[layerName] = layer;
      sortedLayers.Add(layerName);
      sortedLayers.Sort((a, b) => layers[a].Priority > layers[b].Priority ? 1 : -1);
    }
    child.Initialize();
    layer.Instances.Add(child);
  }

  public void RemoveChild(GameObject child, string layerName = DefaultLayerName)
  {
    if (layers.TryGetValue(layerName, out ObjectLayer? value))
    {
      value.Instances.Remove(child);
    }
  }

  public void AddLayer(string layerName, LayerPriority priority = LayerPriority.Default)
  {
    if (!layers.ContainsKey(layerName))
    {
      ObjectLayer newLayer = new(layerName) { Priority = priority };
      layers.Add(layerName, newLayer);
      sortedLayers.Add(layerName);
      sortedLayers.Sort((a, b) => layers[a].Priority.CompareTo(layers[b].Priority));
    }
  }

  public void RemoveLayer(string layerName)
  {
    if (layers.TryGetValue(layerName, out ObjectLayer? value))
    {
      value.Dispose();
      layers.Remove(layerName);
      sortedLayers.Remove(layerName);
    }
  }
  #endregion

  public virtual void Initialize()
  {
    foreach (var layer in layers.Values)
    {
      layer.Initialize();
    }
  }

  public virtual void OnEnter()
  {
    IsActive = true;
    IsPaused = false;
  }

  public virtual void OnExit()
  {
    IsActive = false;
  }

  public virtual void OnPause()
  {
    IsPaused = true;
  }

  public virtual void OnResume()
  {
    IsPaused = false;
  }

  public virtual void Update(float dt)
  {
    if (!IsActive || IsPaused) return;
    foreach (var layer in layers.Values)
    {
      layer.Update(dt);
    }
  }

  public virtual void Draw()
  {
    if (!IsActive) return;
    foreach (var layerName in sortedLayers)
    {
      layers[layerName].Draw();
    }
  }

  public virtual void Dispose()
  {
    foreach (var layer in layers.Values)
    {
      layer.Dispose();
    }
    layers.Clear();
    IsActive = false;
  }
}