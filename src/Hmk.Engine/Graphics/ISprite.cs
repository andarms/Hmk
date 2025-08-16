using System.Numerics;

namespace Hmk.Engine.Graphics;

public interface ISprite
{
  string TextureName { get; }
  Rectangle Source { get; }
  void Draw(Rectangle destination);
}
