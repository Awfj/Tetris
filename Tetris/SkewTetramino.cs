using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Tetris
{
    internal class SkewTetramino : Tetramino
    {
        public SkewTetramino(GraphicsDevice graphicsDevice, Rectangle background)
        {
            RandomizeOrientation(graphicsDevice, background);
        }

        private void RandomizeOrientation(GraphicsDevice graphicsDevice, Rectangle background)
        {
            //int n = new Random().Next(0, 2);
            int n = 0; // TODO: remove this
            /*switch (n)
            {
                case 0:
                    Column = new Random().Next(0, Constants.TotalColumns - 3);
                    Width = Constants.BlockDimension * 4;
                    Height = Constants.BlockDimension;
                    break;
                default:
                    Column = new Random().Next(0, Constants.TotalColumns);
                    Width = Constants.BlockDimension;
                    Height = Constants.BlockDimension * 4;
                    break;
            }*/

            /*Columns = 2;
            Rows = 3;

            Width = Constants.BlockDimension * Columns;
            Height = Constants.BlockDimension * Rows;

            for (int i = 0; i < Columns; i++)
            {
                Blocks.Add(new List<Block>());

                for (int j = 0; j < Rows; j++)
                {
                    if (i == 0 && j == 0 || 
                        i == 1 && j == 2) 
                        continue;

                    Block block = new(
                    background.X + Constants.BlockDimension * (Column + i),
                    background.Y + Constants.BlockDimension * (Row + j),
                    Column + i,
                    Row + j,
                    graphicsDevice);

                    Blocks[i].Add(block);
                }
            }*/

            Columns = 3;
            Rows = 2;

            Width = Constants.BlockDimension * Columns;
            Height = Constants.BlockDimension * Rows;

            for (int i = 0; i < Columns; i++)
            {
                Blocks.Add(new List<Block>());

                for (int j = 0; j < Rows; j++)
                {
                    if (i == 0 && j == 1 ||
                        i == 2 && j == 0)
                        continue;

                    Block block = new(
                    background.X + Constants.BlockDimension * (Column + i),
                    background.Y + Constants.BlockDimension * (Row + j),
                    Column + i,
                    Row + j,
                    graphicsDevice);

                    Blocks[i].Add(block);
                }
            }
        }
    }
}
