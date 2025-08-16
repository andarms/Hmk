using System.Numerics;
using Hmk.Engine.Resources;
using Hmk.Engine.Serializer;

namespace Hmk.Engine.Graphics;

/// <summary>
/// Nine-slice (nine-patch) sprite resource. Provides a helper to draw a resizable UI/frame
/// by splitting a source rectangle into 9 regions using border thickness.
/// </summary>
public class NinePatchSprite : Resource, ISprite
{
  [Save]
  public string TextureName { get; set; } = string.Empty;

  /// <summary>
  /// Source rectangle within the texture for the full 9-slice region.
  /// </summary>
  [Save]
  public Rectangle Source { get; set; }

  /// <summary>
  /// Border thickness in pixels from the left, top, right, bottom edges of the Source.
  /// These define the fixed-size corners and edges.
  /// </summary>
  [Save]
  public float Left { get; set; }
  [Save]
  public float Top { get; set; }
  [Save]
  public float Right { get; set; }
  [Save]
  public float Bottom { get; set; }

  /// <summary>
  /// Origin/pivot in destination pixels (same semantics as Sprite.Anchor for DrawTexturePro origin).
  /// Use half of your destination size to center around a point.
  /// </summary>
  [Save]
  public Vector2 Anchor { get; set; } = new(0, 0);

  [Save]
  public Color Tint { get; set; } = Color.White;

  [Save]
  public float Rotation { get; set; } = 0f;

  private Texture2D Texture => ResourceManager.Textures[TextureName];

  /// <summary>
  /// Draw the nine-slice to a destination rectangle using this resource settings.
  /// </summary>
  public void Draw(Rectangle destination)
  {
    Draw(destination, Anchor, Rotation, Tint);
  }

  /// <summary>
  /// Draw the nine-slice to a destination rectangle with overrides.
  /// </summary>
  public void Draw(Rectangle destination, Vector2 origin, float rotation, Color tint)
  {
    // Ensure valid sizes
    var minW = Left + Right;
    var minH = Top + Bottom;
    var destW = MathF.Max(destination.Width, minW);
    var destH = MathF.Max(destination.Height, minH);

    var destX = destination.X;
    var destY = destination.Y;

    // Source pieces
    float sx = Source.X, sy = Source.Y, sw = Source.Width, sh = Source.Height;
    float cW = MathF.Max(0, sw - Left - Right);
    float cH = MathF.Max(0, sh - Top - Bottom);

    // Destination pieces
    float dCW = MathF.Max(0, destW - Left - Right);
    float dCH = MathF.Max(0, destH - Top - Bottom);

    // Precompute rectangles
    var tex = Texture;

    // Row 1: Top-left, Top, Top-right
    DrawPart(tex, new Rectangle(sx, sy, Left, Top),
      new Rectangle(destX, destY, Left, Top), origin, rotation, tint);
    DrawPart(tex, new Rectangle(sx + Left, sy, cW, Top),
      new Rectangle(destX + Left, destY, dCW, Top), origin, rotation, tint);
    DrawPart(tex, new Rectangle(sx + Left + cW, sy, Right, Top),
      new Rectangle(destX + Left + dCW, destY, Right, Top), origin, rotation, tint);

    // Row 2: Left, Center, Right
    DrawPart(tex, new Rectangle(sx, sy + Top, Left, cH),
      new Rectangle(destX, destY + Top, Left, dCH), origin, rotation, tint);
    DrawPart(tex, new Rectangle(sx + Left, sy + Top, cW, cH),
      new Rectangle(destX + Left, destY + Top, dCW, dCH), origin, rotation, tint);
    DrawPart(tex, new Rectangle(sx + Left + cW, sy + Top, Right, cH),
      new Rectangle(destX + Left + dCW, destY + Top, Right, dCH), origin, rotation, tint);

    // Row 3: Bottom-left, Bottom, Bottom-right
    DrawPart(tex, new Rectangle(sx, sy + Top + cH, Left, Bottom),
      new Rectangle(destX, destY + Top + dCH, Left, Bottom), origin, rotation, tint);
    DrawPart(tex, new Rectangle(sx + Left, sy + Top + cH, cW, Bottom),
      new Rectangle(destX + Left, destY + Top + dCH, dCW, Bottom), origin, rotation, tint);
    DrawPart(tex, new Rectangle(sx + Left + cW, sy + Top + cH, Right, Bottom),
      new Rectangle(destX + Left + dCW, destY + Top + dCH, Right, Bottom), origin, rotation, tint);
  }

  private static void DrawPart(Texture2D texture, Rectangle src, Rectangle dst, Vector2 origin, float rotation, Color tint)
  {
    // Raylib uses a negative width to flip horizontally; ensure non-negative sizes here
    if (src.Width <= 0 || src.Height <= 0 || dst.Width <= 0 || dst.Height <= 0) return;
    DrawTexturePro(texture, src, dst, origin, rotation, tint);
  }
}