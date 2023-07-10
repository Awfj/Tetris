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

            Width = Constants.BlockDimension * 2;
            Height = Constants.BlockDimension * 3;

            for (int i = 0, x = background.X + Constants.BlockDimension * Column; i < 1; i++, x += 20)
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

            for (int i = 0, x = background.X + Constants.BlockDimension * Column; i < 2; i++, x += 20)
            {
                Block block = new(
                    x,
                    background.Y + Constants.BlockDimension,
                    Column + i,
                    Row,
                    graphicsDevice);

                if (i == 1) Blocks.Add(new List<Block>());

                Blocks[i].Add(block);
            }

            for (int i = 1, x = background.X + Constants.BlockDimension * Column; i < 2; i++, x += 20)
            {
                Block block = new(
                    x + Constants.BlockDimension,
                    background.Y + Constants.BlockDimension * 2,
                    Column + i,
                    Row,
                    graphicsDevice);

                Blocks[i].Add(block);
            }

            /*for (int j = 0, y = background.Y + 20; j < 2; j++, y += 20)
            {
                Block block = new(
                    background.X + Constants.BlockDimension * Column,
                    y,
                    Column,
                    Row - j,
                    graphicsDevice);
                Blocks[0].Add(block);
            }*/
        }
    }
}
