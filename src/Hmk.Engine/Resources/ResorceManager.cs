namespace Hmk.Engine.Resources;

using System.Xml.Linq;

public static class ResourcesManager
{
  // Keys are relative paths under Data without extension, using forward slashes.
  public static readonly Dictionary<string, Texture2D> Textures = [];
  public static readonly Dictionary<string, Resource> Resources = [];

  public static void Initialize()
  {
    string resourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
    if (!Directory.Exists(resourcePath))
    {
      // No Data directory present; skip loading resources.
      return;
    }

    LoadTextures(resourcePath);
    LoadResourceFiles(resourcePath);
  }

  private static void LoadTextures(string resourcePath)
  {
    string[] textureFiles = Directory.GetFiles(resourcePath, "*.png", SearchOption.AllDirectories);
    foreach (var file in textureFiles)
    {
      string relativePath = Path.GetRelativePath(resourcePath, file);
      string directory = Path.GetDirectoryName(relativePath) ?? string.Empty;
      string filename = Path.GetFileNameWithoutExtension(file);
      string key = Path.Combine(directory, filename).Replace('\\', '/');
      Textures[key] = LoadTexture(file);
    }
  }

  private static void LoadResourceFiles(string resourcePath)
  {
    string[] resourceFiles = Directory.GetFiles(resourcePath, "*.xres", SearchOption.AllDirectories);
    foreach (var file in resourceFiles)
    {
      try
      {
        var doc = XDocument.Load(file);
        var root = doc.Root;
        if (root == null) continue;

        // Key resolution: explicit Key attribute wins, else use relative path (no extension)
        string relativePath = Path.GetRelativePath(resourcePath, file);
        string withoutExt = Path.Combine(Path.GetDirectoryName(relativePath) ?? string.Empty, Path.GetFileNameWithoutExtension(relativePath));
        string derivedKey = withoutExt.Replace('\\', '/');
        string key = root.Attribute("Key")?.Value?.Trim() ?? derivedKey;

        // Expect a <Resource Type="...">...</Resource> root
        var typeAttr = root.Attribute("Type")?.Value;
        if (string.IsNullOrWhiteSpace(typeAttr))
        {
          // Allow <ResourceRef Path="..."/> to alias another key
          var alias = root.Attribute("Ref")?.Value ?? root.Attribute("Path")?.Value;
          if (!string.IsNullOrWhiteSpace(alias) && Resources.TryGetValue(alias!, out var aliased))
          {
            Resources[key] = aliased;
          }
          continue;
        }

        var t = ResolveType(typeAttr!, typeof(Resource));
        if (t == null) continue;
        if (Activator.CreateInstance(t) is Resource res)
        {
          res.Deserialize(root);
          Resources[key] = res;
        }
      }
      catch
      {
        // Ignore malformed resource files to avoid breaking startup.
      }
    }
  }

  public static bool TryGetResource<T>(string key, out T? resource) where T : class
  {
    resource = null;
    if (Resources.TryGetValue(key, out var res) && res is T match)
    {
      resource = match;
      return true;
    }
    return false;
  }

  public static void Terminate()
  {
    foreach (var texture in Textures.Values)
    {
      UnloadTexture(texture);
    }
    Textures.Clear();
    Resources.Clear();
  }

  private static Type? ResolveType(string nameOrFullName, Type? mustInherit = null)
  {
    foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
    {
      var t = asm.GetType(nameOrFullName, throwOnError: false, ignoreCase: false) ?? asm.GetTypes().FirstOrDefault(tp => tp.Name == nameOrFullName);
      if (t != null && (mustInherit == null || mustInherit.IsAssignableFrom(t)))
      {
        return t;
      }
    }
    return null;
  }
}