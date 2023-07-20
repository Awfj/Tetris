using Microsoft.Xna.Framework;
using System.Collections.Generic;
using static Tetris.Constants;

namespace Tetris
{
    internal static class BlocksMatrix
    {
        public static void NestBlocks(List<Block> blocksColumn, ref Queue<Block> column, int backgroundBottomY)
        {
            Queue<Block> tempColumn = new();

            int n = column.Count;
            for (int j = 0, b = blocksColumn.Count - 1; j < n; j++)
            {
                var block = column.Dequeue();

                if (b >= 0 && tempColumn.Count == TotalRows - blocksColumn[b].Row - 1)
                {
                    Block currentBlock = blocksColumn[b];
                    AddBlock(tempColumn, currentBlock, backgroundBottomY);

                    b--;
                }
                else
                {
                    tempColumn.Enqueue(block);
                }
            }

            column = tempColumn;
        }

        public static void AddBlocks(List<Block> blocksColumn, Queue<Block> column, int rowCount, int nextRow, int backgroundBottomY)
        {
            for (int j = nextRow + rowCount - 1; j >= nextRow; j--)
            {
                Block currentBlock = blocksColumn[j - nextRow];

                AddBlock(column, currentBlock, backgroundBottomY);
            }
        }

        private static void AddBlock(Queue<Block> column, Block currentBlock, int backgroundBottomY)
        {
            AdjustBlockPosition(currentBlock, backgroundBottomY);
            column.Enqueue(currentBlock);
        }

        private static void AdjustBlockPosition(Block currentBlock, int backgroundBottomY)
        {
            Rectangle tempRectangle = currentBlock.Rectangle;
            tempRectangle.Y = backgroundBottomY - Block.Length * (TotalRows - currentBlock.Row);

            currentBlock.Rectangle = tempRectangle;
        }

        public static void LevelRows(Queue<Block> column, int rowCount, int nextRow)
        {
            for (int i = 0; i < rowCount; i++)
            {
                while (column.Count < TotalRows - nextRow)
                {
                    column.Enqueue(null);
                }
            }
        }
    }
}
