using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace Tetris.Tetraminos
{
    internal abstract class Tetramino
    {
        public List<List<Block>> Blocks { get; set; } = new();
        public Color Color { get; set; } = Color.White;
        protected int InitialColumn { get; } = 6;
        protected int InitialRow { get; } = 0;
        protected int Columns { get; set; }
        protected int Rows { get; set; }
        protected int Type { get; set; }
        protected int[,] Skip { get; set; } = new int[0, 0];

        protected abstract void Set();

        protected void Make(GraphicsDevice graphicsDevice, Rectangle background)
        {
            Set();

            for (int i = 0; i < Columns; i++)
            {
                Blocks.Add(new List<Block>());

                for (int j = 0; j < Rows; j++)
                {
                    if (Check(Skip, i, j)) continue;

                    Block newBlock = new(
                        background.X + Constants.BlockDimension * (InitialColumn + i),
                            background.Y + Constants.BlockDimension * (InitialRow + j),
                            InitialColumn + i,
                            InitialRow + j,
                            graphicsDevice);

                    Blocks[i].Add(newBlock);
                }
            }
        }

        protected void Make(GraphicsDevice graphicsDevice, Block block)
        {
            Blocks = new();
            Set();

            for (int i = 0; i < Columns; i++)
            {
                Blocks.Add(new List<Block>());

                for (int j = 0; j < Rows; j++)
                {
                    if (Check(Skip, i, j)) continue;

                    Block newBlock = new(
                            block.Rectangle.X + Constants.BlockDimension * i,
                            block.Rectangle.Y + Constants.BlockDimension * j,
                            block.Column + i,
                            block.Row + j,
                            graphicsDevice);

                    Blocks[i].Add(newBlock);
                }
            }
        }

        private static bool Check(int[,] skip, int i, int j)
        {
            for (int k = 0; k < skip.GetLength(0); k++)
            {
                if (i == skip[k, 0] && j == skip[k, 1])
                {
                    return true;
                }
            }

            return false;
        }

        /*public int Width { get; set; }
        public int Height { get; set; }*/

        public abstract void Rotate(GraphicsDevice graphicsDevice);
    }
}
