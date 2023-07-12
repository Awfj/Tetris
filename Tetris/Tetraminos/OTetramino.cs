using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Tetris.Tetraminos
{
    internal class OTetramino : Tetramino
    {
        public OTetramino(GraphicsDevice graphicsDevice, Rectangle background)
        {
            Columns = 2;
            Rows = 2;

            for (int i = 0; i < Columns; i++)
            {
                Blocks.Add(new List<Block>());

                for (int j = 0; j < Rows; j++)
                {
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

        public override void Rotate() { }
    }
}
