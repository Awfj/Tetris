using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using static Tetris.Constants;

namespace Tetris
{
    internal static class Movement
    {
        public const int Speed = 2;

        public static bool MoveDown(Tetramino element, Queue<Block>[] columns, Rectangle background)
        {
            Direction direction = Direction.Down;

            if (BorderCollision.CheckIfCollidesWithSideBorder(element, direction)) return true;
            if (BlockCollision.CheckIfCollidesWithBlock(element, columns, direction)) return true;

            MoveElement(element, direction);
            ChangeRow(element, background);

            return false;
        }

        public static void MoveHorizontally(Tetramino element, Queue<Block>[] columns, Direction direction, ref bool keyDelayActive)
        {
            if (keyDelayActive) return;
            if (BorderCollision.CheckIfCollidesWithSideBorder(element, direction)) return;
            if (BlockCollision.CheckIfCollidesWithBlock(element, columns, direction)) return;


            MoveElement(element, direction);
            ChangeColumn(element, direction);

            keyDelayActive = true;
        }

        private static void MoveElement(Tetramino element, Direction direction)
        {
            foreach (var column in element.Blocks)
            {
                foreach (var block in column)
                {
                    var s = block.Rectangle;

                    if (direction == Direction.Left)
                    {
                        s.X = GetUpdatedToLeftX(s);
                    }
                    else if (direction == Direction.Right)
                    {
                        s.X = GetUpdatedToRightX(s);
                    }
                    else if (direction == Direction.Down)
                    {
                        s.Y += Speed; // if Speed < TotalRows
                    }

                    block.Rectangle = s;
                }
            }
        }

        private static int GetUpdatedToLeftX(Rectangle element)
        {
            return element.X - 20;
        }

        private static int GetUpdatedToRightX(Rectangle element)
        {
            return element.X + 20;
        }

        private static void ChangeRow(Tetramino element, Rectangle background)
        {
            foreach (var column in element.Blocks)
            {
                foreach (var block in column)
                {
                    block.Row = TotalRows - (int)Math.Ceiling(((background.Y + background.Height) - block.Rectangle.Y) / (double)TotalRows);
                }
            }
        }

        private static void ChangeColumn(Tetramino element, Direction direction)
        {
            foreach (var column in element.Blocks)
            {
                foreach (var block in column)
                {
                    if (direction == Direction.Right)
                    {
                        block.Column++;
                    }
                    else if (direction == Direction.Left)
                        block.Column--;
                }
            }
        }
    }
}
