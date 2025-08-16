using System.Numerics;
using System.Xml.Linq;
using Hmk.Engine.Resources;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Graphics;


public class Sprite : Resource, ISprite
{
  [Save]
  public string TextureName { get; set; } = string.Empty;
  [Save]
  public Rectangle Source { get; set; }

  [Save]
  public Vector2 Anchor { get; set; } = new Vector2(0, 0);
  [Save]
  public Color Tint { get; set; } = Color.White;
  [Save]
  public float Rotation { get; set; } = 0;

  private Texture2D Texture => ResourceManager.Textures[TextureName];

  public void Draw(Rectangle destination)
  {
    Draw(destination, Anchor, Rotation, Tint);
  }

  public void Draw(Rectangle destination, Vector2 origin, float rotation, Color tint)
  {
    DrawTexturePro(Texture, Source, destination, origin, rotation, tint);
  }
}