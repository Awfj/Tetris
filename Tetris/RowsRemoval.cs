using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    internal static class RowsRemoval
    {
        public static void RemoveFullRows(Queue<Block>[] columns)
        {
            Queue<int> rowsToRemove = FindFullRows(columns);

            if (rowsToRemove.Count == 0) return;

            for (int i = 0; i < columns.Length; i++)
            {
                // temporary queue to store the moved blocks
                Queue<Block> temp = new();
                int rows = columns[i].Count;
                int removedRows = 0;

                for (int j = 0; j < rows; j++)
                {
                    Block block = columns[i].Dequeue();

                    // if the current row is to be removed then skip it
                    // and move the block if its not null
                    if (removedRows < rowsToRemove.Count &&
                        j == rowsToRemove.ElementAt(removedRows))
                    {
                        removedRows++;
                    }
                    else
                    {
                        if (block is not null)
                        {
                            MoveBlock(block, removedRows);
                        }

                        temp.Enqueue(block);
                    }
                }

                columns[i] = temp;
            }

            rowsToRemove.Clear();
        }

        private static Queue<int> FindFullRows(Queue<Block>[] columns)
        {
            Queue<int> rowsToRemove = new();

            int minColumnHeight = FindMinColumnHeight(columns);

            for (int i = 0; i < minColumnHeight; i++)
            {
                if (CheckIfRowIsFull(columns, i))
                {
                    rowsToRemove.Enqueue(i);
                }
            }

            return rowsToRemove;
        }

        private static void MoveBlock(Block block, int rows)
        {
            Rectangle rect = block.Rectangle;
            rect.Y += Block.Length * rows;
            block.Rectangle = rect;
            block.Row += rows;
        }

        private static int FindMinColumnHeight(Queue<Block>[] columns)
        {
            int min = columns[0].Count;

            for (int i = 1; i < columns.Length; i++)
            {
                int columnHeight = columns[i].Count;

                if (columnHeight < min)
                {
                    min = columnHeight;
                }
            }

            return min;
        }

        private static bool CheckIfRowIsFull(Queue<Block>[] columns, int row)
        {
            foreach (var column in columns)
            {
                if (column.ElementAt(row) == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
