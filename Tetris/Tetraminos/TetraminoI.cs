using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Tetris
{
    internal class TetraminoI : Tetramino
    {
        public TetraminoI(GraphicsDevice graphicsDevice, Rectangle background)
        {
            Color = Color.Aqua;
            Type = new Random().Next(0, 4);

            Initialize(graphicsDevice, Color, background);
        }

        public override void Rotate(GraphicsDevice graphicsDevice, Queue<Block>[] columns)
        {
            Block block = Blocks[0][0];
            Rectangle rectangle = block.Rectangle;

            int column = block.Column;
            int row = block.Row;

            switch (Type)
            {
                case 1:
                    column -= 2;
                    row += 2;
                    break;
                case 2:
                    column += 1;
                    row -= 2;
                    break;
                case 3:
                    column -= 1;
                    row += 1;
                    break;
                default:
                    column += 2;
                    row -= 1;
                    break;
            }

            if (ShouldRotate(columns, column, row) == false)
                return;

            switch (Type)
            {
                case 1:
                    rectangle.X -= Block.Length * 2;
                    rectangle.Y += Block.Length * 2;
                    break;
                case 2:
                    rectangle.X += Block.Length;
                    rectangle.Y -= Block.Length * 2;
                    break;
                case 3:
                    rectangle.X -= Block.Length;
                    rectangle.Y += Block.Length;
                    break;
                default:
                    rectangle.X += Block.Length * 2;
                    rectangle.Y -= Block.Length;
                    break;
            }

            Type = GetNextType();
            Blocks[0][0].Column = column;
            Blocks[0][0].Row = row;
            Blocks[0][0].Rectangle = rectangle;
            Initialize(graphicsDevice, Color, Blocks[0][0]);
        }

        private bool ShouldRotate(Queue<Block>[] columns, int column, int row)
        {
            int nextType = GetNextType();
            (int newColumns, int newRows) = GetNewSize(nextType);

            if (column + newColumns > Constants.TotalColumns ||
                column < 0 ||
                row + newRows > Constants.TotalRows - 1)
            {
                return false;
            }

            foreach (var rowQueue in columns)
            {
                foreach (var block in rowQueue)
                {
                    if (block == null)
                        continue;

                    if (block.Row == row && block.Column == column)
                        return false;
                }

            }

            return true;
        }

        protected override void SetProperties()
        {
            (int columns, int rows) = GetNewSize(Type);
            Columns = columns;
            Rows = rows;
        }

        private (int columns, int rows) GetNewSize(int type)
        {
            return type switch
            {
                1 or 3 => (1, 4),
                _ => (4, 1),
            };
        }

        private int GetNextType()
        {
            return Type switch
            {
                1 => 2,
                2 => 3,
                3 => 0,
                _ => 1,
            };
        }
    }
}
