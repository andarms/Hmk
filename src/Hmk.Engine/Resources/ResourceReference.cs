using Hmk.Engine.Serializer;

namespace Hmk.Engine.Resources;

public class ResourceReference<T> : Resource where T : Resource
{
  [Save]
  public string Path { get; set; } = string.Empty;

  // Optional eager field in case something sets it directly
  public T? Resource { get; set; }

  public T Value
  {
    get
    {
      if (Resource is T set) return set;
      if (ResourcesManager.TryGetResource<T>(Path, out var found) && found is T ok)
      {
        return ok;
      }
      // Last chance: dictionary lookup cast
      if (ResourcesManager.Resources.TryGetValue(Path, out var any) && any is T casted)
      {
        return casted;
      }
      throw new InvalidOperationException($"Resource '{Path}' is not loaded or of wrong type {typeof(T).Name}.");
    }
  }
}