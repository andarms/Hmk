namespace Hmk.Engine.Resources;

public static class ResourceManager
{
  public static readonly Dictionary<string, Texture2D> Textures = [];

  public static void Initialize()
  {
    string resourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
    if (!Directory.Exists(resourcePath))
    {
      // No Data directory present; skip loading resources.
      return;
    }
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

  public static void Terminate()
  {
    foreach (var texture in Textures.Values)
    {
      UnloadTexture(texture);
    }
    Textures.Clear();
  }
}