using System.Collections.Generic;
using System.Linq;
using static Tetris.Constants;

namespace Tetris
{
    internal static class BlockCollision
    {
        public static bool CheckIfCollidesWithBlock(ITetramino element, Queue<Block>[] columns, Direction direction)
        {
            return direction switch
            {
                Direction.Left => CheckIfCollidesWithBlockLeft(element, columns),
                Direction.Right => CheckIfCollidesWithBlockRight(element, columns),
                Direction.Down => CheckIfCollidesWithBlockDown(element, columns),
                _ => false,
            };
        }

        private static bool CheckIfCollidesWithBlockLeft(ITetramino element, Queue<Block>[] columns)
        {
            foreach (var column in element.Blocks)
            {
                foreach (var block in column)
                {
                    int nextColumn = block.Column - 1;
                    int row = TotalRows - block.Row - 1;

                    if (row >= columns[nextColumn].Count) continue;
                    if (columns[nextColumn].ElementAt(TotalRows - block.Row - 1) != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool CheckIfCollidesWithBlockRight(ITetramino element, Queue<Block>[] columns)
        {
            foreach (var column in element.Blocks)
            {
                foreach (var block in column)
                {
                    int nextColumn = block.Column + 1;
                    int row = TotalRows - block.Row - 1;

                    if (row >= columns[nextColumn].Count) continue;
                    if (columns[nextColumn].ElementAt(TotalRows - block.Row - 1) != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool CheckIfCollidesWithBlockDown(ITetramino element, Queue<Block>[] columns)
        {
            foreach (var column in element.Blocks)
            {
                foreach (var block in column)
                {
                    int nextRow = TotalRows - block.Row - 2;
                    if (columns[block.Column].Count <= nextRow) continue;
                    if (columns[block.Column].ElementAt(nextRow) != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
