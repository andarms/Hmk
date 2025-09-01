using System.Numerics;
using Hamaze.Engine.Core;
using DotTiled;

namespace Hamaze.Game.Scenes;

public class TmxMapSystem(string mapPath = "Assets/Maps/map_001.tmx") : WorldSystem
{
  private Map? tmxMap;
  private readonly Dictionary<uint, Texture2D> tilesetTextures = [];
  private readonly string mapPath = mapPath;

  public override void Initialize()
  {
    base.Initialize();
    LoadTmxMap();
  }

  private void LoadTmxMap()
  {
    try
    {
      // Get the full path to the map file
      string fullMapPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, mapPath);

      if (!File.Exists(fullMapPath))
      {
        Console.WriteLine($"TMX map file not found: {fullMapPath}");
        return;
      }

      // Load the TMX map using DotTiled
      var loader = DotTiled.Serialization.Loader.Default();
      tmxMap = loader.LoadMap(fullMapPath);

      Console.WriteLine($"Loaded TMX map: {tmxMap.Width}x{tmxMap.Height}, Tile size: {tmxMap.TileWidth}x{tmxMap.TileHeight}");
      Console.WriteLine($"Layers: {tmxMap.Layers.Count}, Tilesets: {tmxMap.Tilesets.Count}");

      // Load tileset textures
      LoadTilesetTextures();
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error loading TMX map: {ex.Message}");
    }
  }

  private void LoadTilesetTextures()
  {
    if (tmxMap == null) return;

    foreach (var tileset in tmxMap.Tilesets)
    {
      try
      {
        // Handle tileset image path
        string imagePath = "";
        if (tileset.Image.HasValue)
        {
          imagePath = tileset.Image.Value.Source;
        }

        // Convert to relative path from Assets folder
        if (imagePath.StartsWith("Tilesets/"))
        {
          // Use the existing texture from ResourceManager
          string textureKey = imagePath.Replace(".aseprite", "").Replace(".png", "");

          if (ResourcesManager.Textures.TryGetValue(textureKey, out Texture2D value))
          {
            tilesetTextures[tileset.FirstGID] = value;
            Console.WriteLine($"Loaded tileset texture: {textureKey} for GID {tileset.FirstGID}");
          }
          else
          {
            Console.WriteLine($"Tileset texture not found in ResourceManager: {textureKey}");
          }
        }
        else if (!string.IsNullOrEmpty(imagePath))
        {
          // Try to load the texture directly
          string fullImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", imagePath);

          if (File.Exists(fullImagePath))
          {
            tilesetTextures[tileset.FirstGID] = LoadTexture(fullImagePath);
            Console.WriteLine($"Loaded tileset texture: {fullImagePath} for GID {tileset.FirstGID}");
          }
          else
          {
            Console.WriteLine($"Tileset image not found: {fullImagePath}");
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error loading tileset texture: {ex.Message}");
      }
    }
  }

  public override void Draw(IEnumerable<IEntity> _)
  {
    if (tmxMap == null) return;

    foreach (var layer in tmxMap.Layers)
    {
      if (layer is TileLayer tileLayer)
      {
        DrawTileLayer(tileLayer);
      }
    }
  }

  private void DrawTileLayer(TileLayer tileLayer)
  {
    if (tmxMap == null) return;

    for (int y = 0; y < tileLayer.Height; y++)
    {
      for (int x = 0; x < tileLayer.Width; x++)
      {
        uint gid = 0;
        if (tileLayer.Data.HasValue && tileLayer.Data.Value.GlobalTileIDs.HasValue)
        {
          gid = tileLayer.Data.Value.GlobalTileIDs.Value[y * tileLayer.Width + x];
        }

        // Skip empty tiles (GID 0)
        if (gid == 0) continue;

        // Find the appropriate tileset for this GID
        var tileset = GetTilesetForGid(gid);
        if (tileset == null) continue;

        // Get the texture for this tileset
        if (!tilesetTextures.TryGetValue(tileset.FirstGID, out var texture))
          continue;

        // Calculate local tile ID within the tileset
        uint localId = gid - tileset.FirstGID;

        // Calculate source rectangle in the tileset
        var sourceRect = GetTileSourceRect(tileset, localId);

        // Calculate destination position
        Vector2 position = new(
            x * tmxMap.TileWidth,
            y * tmxMap.TileHeight
        );

        // Draw the tile
        DrawTextureRec(texture, sourceRect, position, Raylib_cs.Color.White);
      }
    }
  }

  private Tileset? GetTilesetForGid(uint gid)
  {
    if (tmxMap == null) return null;

    // Find the tileset that contains this GID
    Tileset? foundTileset = null;
    foreach (var tileset in tmxMap.Tilesets)
    {
      if (gid >= tileset.FirstGID && (foundTileset == null || tileset.FirstGID > foundTileset.FirstGID))
      {
        foundTileset = tileset;
      }
    }

    return foundTileset;
  }

  private Rectangle GetTileSourceRect(Tileset tileset, uint localId)
  {
    // Calculate the position of the tile in the tileset image
    int tilesPerRow = (int)tileset.Columns;
    int tileX = (int)(localId % tilesPerRow);
    int tileY = (int)(localId / tilesPerRow);

    return new Rectangle(
        tileX * tileset.TileWidth,
        tileY * tileset.TileHeight,
        tileset.TileWidth,
        tileset.TileHeight
    );
  }

  public Map? GetMap() => tmxMap;

  public Vector2 GetMapSize()
  {
    if (tmxMap == null) return Vector2.Zero;
    return new Vector2(tmxMap.Width * tmxMap.TileWidth, tmxMap.Height * tmxMap.TileHeight);
  }
}
