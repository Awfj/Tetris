using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris.Tetraminos
{
    internal class TetraminoO : Tetramino
    {
        public TetraminoO(GraphicsDevice graphicsDevice, Rectangle background, int type)
        {
            Color = Color.Yellow;
            Type = type;

            Make(graphicsDevice, background);
        }

        public override void Rotate(GraphicsDevice graphicsDevice)
        {
            Make(graphicsDevice, Blocks[0][0]);
        }

        protected override void Set()
        {
            Columns = 2;
            Rows = 2;
        }
    }
}
