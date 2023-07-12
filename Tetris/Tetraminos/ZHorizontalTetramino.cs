using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.ComponentModel;

namespace Tetris.Tetraminos
{
    internal class ZHorizontalTetramino : Tetramino
    {
        public ZHorizontalTetramino(GraphicsDevice graphicsDevice, Rectangle background)
        {
            Columns = 3;
            Rows = 2;

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

        public ZHorizontalTetramino(GraphicsDevice graphicsDevice, Block block)
        {
            Columns = 2;
            Rows = 3;
            Column = block.Column;
            Row = block.Row;

            for (int i = 0; i < Columns; i++)
            {
                Blocks.Add(new List<Block>());

                for (int j = 0, k = Rows - 1; j < Rows; j++, k--)
                {
                    if (i == 0 && j == 0 ||
                        i == 1 && j == 2)
                        continue;

                    Block newBlock = new(
                    block.Rectangle.X + Constants.BlockDimension * i,
                    block.Rectangle.Y + Constants.BlockDimension * j,
                    Column + i,
                    Row + j,
                    graphicsDevice);

                    Blocks[i].Add(newBlock);
                }
            }
        }

        public override void Rotate()
        {
            
        }

        /*public ZHorizontalTetramino(GraphicsDevice graphicsDevice, int x, int y)
        {
            Columns = 3;
            Rows = 2;

            for (int i = 0; i < Columns; i++)
            {
                Blocks.Add(new List<Block>());

                for (int j = 0; j < Rows; j++)
                {
                    if (i == 0 && j == 1 ||
                        i == 2 && j == 0)
                        continue;

                    Block block = new(
                    x + Constants.BlockDimension * (Column + i),
                    y + Constants.BlockDimension * (Row + j),
                    Column + i,
                    Row + j,
                    graphicsDevice);

                    Blocks[i].Add(block);
                }
            }
        }*/
    }
}
