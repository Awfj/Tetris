using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris.Tetraminos
{
    internal class TetraminoZ : Tetramino
    {

        public TetraminoZ(GraphicsDevice graphicsDevice, Rectangle background, int type)
        {
            Color = Color.Red;
            Type = type;

            Make(graphicsDevice, background);
        }

        public override void Rotate(GraphicsDevice graphicsDevice)
        {
            if (Type == 0) Type = 1;
            else Type = 0;

            Make(graphicsDevice, Blocks[0][0]);
        }

        protected override void Set()
        {
            switch (Type)
            {
                case 1:
                    Columns = 2;
                    Rows = 3;
                    Skip = new[,] { { 0, 0 }, { 1, 2 } };
                    break;
                default:
                    Columns = 3;
                    Rows = 2;
                    Skip = new[,] { { 0, 1 }, { 2, 0 } };
                    break;
            }
        }
    }
}
