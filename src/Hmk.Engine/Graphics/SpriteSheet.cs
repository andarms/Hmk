using System.Numerics;
using Hmk.Engine.Core;
using Hmk.Engine.Resources;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Graphics;

public class SpriteSheet : Resource, ISprite
{
  [Save]
  public string TextureName { get; set; } = string.Empty;

  [Save]
  public int FrameWidth { get; set; } = 1;

  [Save]
  public int FrameHeight { get; set; } = 1;

  Texture2D Texture => ResourcesManager.Textures[TextureName];
  int TotalFrames => Texture.Width / FrameWidth * (Texture.Height / FrameHeight);

  public Rectangle Source { get; set; } = new Rectangle(0, 0, 0, 0);

  public Rectangle GetFrame(int index)
  {
    if (index < 0 || index >= TotalFrames)
    {
      throw new ArgumentOutOfRangeException(nameof(index), "Frame index is out of range.");
    }
    int columns = Texture.Width / FrameWidth;
    int x = index % columns * FrameWidth;
    int y = index / columns * FrameHeight;
    return new Rectangle(x, y, FrameWidth, FrameHeight);
  }

  public void SetFrame(int index)
  {
    if (index < 0 || index >= TotalFrames)
    {
      throw new ArgumentOutOfRangeException(nameof(index), "Frame index is out of range.");
    }
    Source = GetFrame(index);
  }

  public void Draw(Rectangle destination)
  {
    DrawTexturePro(Texture, Source, destination, Vector2.Zero, 0f, Color.White);
  }
}