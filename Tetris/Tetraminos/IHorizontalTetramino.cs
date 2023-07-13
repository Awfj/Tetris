using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Tetris.Tetraminos
{
    internal class IHorizontalTetramino : Tetramino
    {
        public IHorizontalTetramino(GraphicsDevice graphicsDevice, Rectangle background)
        {
            Columns = 4;
            Rows = 1;

            for (int i = 0; i < Columns; i++)
            {
                Blocks.Add(new List<Block>());

                for (int j = 0; j < Rows; j++)
                {
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

        public override void Rotate(GraphicsDevice graphicsDevice) { }
    }
}
