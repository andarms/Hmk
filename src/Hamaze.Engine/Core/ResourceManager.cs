namespace Hamaze.Engine.Core;

public static class ResourcesManager
{
  // Keys are relative paths under Data without extension, using forward slashes.
  public static readonly Dictionary<string, Texture2D> Textures = [];

  public static void Initialize()
  {
    string resourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets");
    if (!Directory.Exists(resourcePath))
    {
      // No Data directory present; skip loading resources.
      return;
    }

    LoadTextures(resourcePath);
  }

  internal static void Terminate()
  {
    foreach (var texture in Textures.Values)
    {
      UnloadTexture(texture);
    }
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
}