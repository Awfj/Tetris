using Microsoft.Xna.Framework;

namespace Tetris;

internal static class TetraminoFactory
{
    public static Tetramino GetTetramino(Rectangle backgroundRectangle, int type)
    {
        return type switch
        {
            0 => new TetraminoI(backgroundRectangle),
            1 => new TetraminoZ(backgroundRectangle),
            2 => new TetraminoJ(backgroundRectangle),
            3 => new TetraminoL(backgroundRectangle),
            4 => new TetraminoS(backgroundRectangle),
            5 => new TetraminoT(backgroundRectangle),
            _ => new TetraminoO(backgroundRectangle),
        };
    }
}
