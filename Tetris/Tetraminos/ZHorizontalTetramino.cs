using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace Tetris.Tetraminos
{
    internal class ZHorizontalTetramino : Tetramino
    {
        public ZHorizontalTetramino(GraphicsDevice graphicsDevice, Rectangle background, int type)
        {
            Color = Color.Red;

            switch (type)
            {
                case 0:
                    Columns = 3;
                    Rows = 2;
                    break;
                case 1:
                    Columns = 2;
                    Rows = 3;
                    break;
            }

            for (int i = 0; i < Columns; i++)
            {
                Blocks.Add(new List<Block>());

                for (int j = 0; j < Rows; j++)
                {
                    if (Check(type, i, j)) continue;

                    Block block = new(
                    background.X + Constants.BlockDimension * (InitialColumn + i),
                    background.Y + Constants.BlockDimension * (InitialRow + j),
                    InitialColumn + i,
                    InitialRow + j,
                    graphicsDevice);

                    Blocks[i].Add(block);
                }
            }
        }

        private bool Check(int type, int i, int j)
        {
            int[,] skip;

            switch (type)
            {
                case 1:
                    skip = new[,] { { 0, 1 }, { 2, 0 } };
                    break;
                default:
                    skip = new[,] { { 0, 0 }, { 1, 2 } };
                    break;
            }


            for (int k = 0; k < skip.GetLength(0); k++)
            {
                if (i == skip[k, 0] && j == skip[k, 1])
                {
                    return true;
                }
            }

            return false;
        }

        private void Rotate0(GraphicsDevice graphicsDevice)
        {

        }

        /*private void Rotate90(GraphicsDevice graphicsDevice)
        {
            Columns = 2;
            Rows = 3;

            Block block = Blocks[0][0];
            Blocks = new();

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
                    block.Column + i,
                    block.Row + j,
                    graphicsDevice);

                    Blocks[i].Add(newBlock);
                }
            }
        }*/

        private void Make(GraphicsDevice graphicsDevice, int type, bool isNew)
        {
            Block block = Blocks[0][0];
            Blocks = new();

            for (int i = 0; i < Columns; i++)
            {
                Blocks.Add(new List<Block>());

                for (int j = 0, k = Rows - 1; j < Rows; j++, k--)
                {
                    if (Check(type, i, j)) continue;

                    Block newBlock;
                    if (isNew)
                    {
                        newBlock = new(
                        background.X + Constants.BlockDimension * (InitialColumn + i),
                            background.Y + Constants.BlockDimension * (InitialRow + j),
                            InitialColumn + i,
                            InitialRow + j,
                            graphicsDevice);
                    } 
                    else
                    {
                        newBlock = new(
                            block.Rectangle.X + Constants.BlockDimension * i,
                            block.Rectangle.Y + Constants.BlockDimension * j,
                            block.Column + i,
                            block.Row + j,
                            graphicsDevice);
                    }

                    Blocks[i].Add(newBlock);
                }
            }
        }

        public override void Rotate(GraphicsDevice graphicsDevice)
        {

        }
    }
}
