﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Tetris
{
    internal class TetraminoI : Tetramino
    {
        public TetraminoI(GraphicsDevice graphicsDevice, Rectangle background)
        {
            Color = Color.Aqua;
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

                    rectangle.X -= Block.Length * 2;
                    rectangle.Y += Block.Length * 2;
                    block.Column -= 2;
                    block.Row += 2;
                    break;
                case 2:
                    Type = 3;

                    rectangle.X += Block.Length;
                    rectangle.Y -= Block.Length * 2;
                    block.Column += 1;
                    block.Row -= 2;
                    break;
                case 3:
                    Type = 0;

                    rectangle.X -= Block.Length;
                    rectangle.Y += Block.Length;
                    block.Column -= 1;
                    block.Row += 1;
                    break;
                default:
                    Type = 1;

                    rectangle.X += Block.Length * 2;
                    rectangle.Y -= Block.Length;
                    block.Column += 2;
                    block.Row -= 1;
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
                    Columns = 1;
                    Rows = 4;
                    break;
                case 2:
                default:
                    Columns = 4;
                    Rows = 1;
                    break;
            }
        }
    }
}
