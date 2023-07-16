using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Tetris
{
    internal class TetraminoT : Tetramino
    {
        public TetraminoT(GraphicsDevice graphicsDevice, Rectangle background)
        {
            Color = Color.Violet;
            Type = new Random().Next(0, 4);

            Initialize(graphicsDevice, Color, background);
        }

        public override void Rotate(GraphicsDevice graphicsDevice)
        {
            Block block = Blocks[0][0];
            Rectangle rectangle = block.Rectangle;

            switch (Type)
            {
                case 1:
                    Type = 2;

                    rectangle.X -= Block.Length;
                    rectangle.Y += Block.Length;
                    block.Column -= 1;
                    block.Row += 1;
                    break;
                case 2:
                    Type = 3;

                    block.Row -= 1;
                    break;
                case 3:
                    Type = 0;
                    break;
                case 0:
                    Type = 1;

                    rectangle.X += Block.Length;
                    rectangle.Y -= Block.Length;
                    block.Column += 1;
                    break;
            }

            Blocks[0][0].Rectangle = rectangle;
            Initialize(graphicsDevice, Color, Blocks[0][0]);
        }

        protected override void SetProperties()
        {
            switch (Type)
            {
                case 1:
                case 3:
                    Columns = 2;
                    Rows = 3;
                    break;
                case 2:
                default:
                    Columns = 3;
                    Rows = 2;
                    break;
            }

            switch (Type)
            {
                case 1:
                    Skip = new[,] { { 1, 0 }, { 1, 2 } };
                    break;
                case 2:
                    Skip = new[,] { { 0, 1 }, { 2, 1 } };
                    break;
                case 3:
                    Skip = new[,] { { 0, 0 }, { 0, 2 } };
                    break;
                default:
                    Skip = new[,] { { 0, 0 }, { 2, 0 } };
                    break;
            }
        }
    }
}
