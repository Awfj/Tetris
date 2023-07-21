using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    internal static class TetraminoFactory
    {
        public static Tetramino GetTetramino(GraphicsDevice graphicsDevice, Rectangle backgroundRectangle, int type)
        {
            return type switch
            {
                0 => new TetraminoI(graphicsDevice, backgroundRectangle),
                1 => new TetraminoZ(graphicsDevice, backgroundRectangle),
                2 => new TetraminoJ(graphicsDevice, backgroundRectangle),
                3 => new TetraminoL(graphicsDevice, backgroundRectangle),
                4 => new TetraminoS(graphicsDevice, backgroundRectangle),
                5 => new TetraminoT(graphicsDevice, backgroundRectangle),
                _ => new TetraminoO(graphicsDevice, backgroundRectangle),
            };
        }
    }
}
