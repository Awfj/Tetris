using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Tetris.Tetraminos
{
    internal class TetraminoI : Tetramino
    {
        public TetraminoI(GraphicsDevice graphicsDevice, Rectangle background)
        {
            Color = Color.Aqua;
            Type = new Random().Next(0, 2);

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
                    Columns = 1;
                    Rows = 4;
                    break;
                default:
                    Columns = 4;
                    Rows = 1;
                    break;
            }
        }
    }
}
