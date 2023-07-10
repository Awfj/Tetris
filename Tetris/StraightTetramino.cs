using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Tetris
{
    internal class StraightTetramino : Tetramino
    {
        public StraightTetramino(GraphicsDevice graphicsDevice, Rectangle background)
        {
            RandomizeOrientation(graphicsDevice, background);

            Test = 5;
        }

        public int Test { get; set; } // TODO: remove this

        private void RandomizeOrientation(GraphicsDevice graphicsDevice, Rectangle background)
        {
            int n = new Random().Next(0, 2);
            switch (n)
            {
                case 0:
                    //Column = new Random().Next(0, Constants.TotalColumns - 3);
                    Width = Constants.BlockDimension * 4;
                    Height = Constants.BlockDimension;

                    // horizontal
                    for (int i = 0, x = background.X + Constants.BlockDimension * Column; i < 4; i++, x += 20)
                    {
                        Block block = new(
                            x,
                            background.Y,
                            Column + i,
                            Row,
                            graphicsDevice);

                        Blocks.Add(new List<Block>());
                        Blocks[i].Add(block);
                    }
                    break;
                default:
                    //Column = new Random().Next(0, Constants.TotalColumns);
                    Width = Constants.BlockDimension;
                    Height = Constants.BlockDimension * 4;

                    // vertial
                    Blocks.Add(new List<Block>());
                    for (int j = 0, y = background.Y; j < 4; j++, y += 20)
                    {
                        Block block = new(
                            background.X + Constants.BlockDimension * Column,
                            y,
                            Column,
                            Row - j,
                            graphicsDevice);
                        Blocks[0].Add(block);
                    }
                    break;
            }
        }
    }
}
