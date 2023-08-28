using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Tetris;

internal class TetraminoS : Tetramino
{
    public TetraminoS(Rectangle background)
    {
        Color = Color.Green;
        Type = new Random().Next(0, 4);

        Initialize(Color, background);
    }

    public override void Rotate(Queue<Block>[] columnse)
    {
        Block block = Blocks[0][0];
        Rectangle rectangle = block.Rectangle;

        switch (Type)
        {
            case 1:
                Type = 2;

                rectangle.X -= Block.Length;
                rectangle.Y += Block.Length * 2;
                block.Column -= 1;
                block.Row += 1;
                break;
            case 2:
                Type = 3;

                rectangle.Y -= Block.Length * 2;
                block.Row -= 1;
                break;
            case 3:
                Type = 0;

                rectangle.Y += Block.Length * 2;
                block.Row += 1;
                break;
            default:
                Type = 1;

                rectangle.X += Block.Length;
                rectangle.Y -= Block.Length * 2;
                block.Column += 1;
                block.Row -= 1;
                break;
        }

        Blocks[0][0].Rectangle = rectangle;
        Initialize(Color, Blocks[0][0]);
    }

    protected override void SetProperties()
    {
        (int column, int row) = GetLocation();
        Columns = column;
        Rows = row;

        Skip = Type switch
        {
            1 or 3 => new[,] { { 0, 2 }, { 1, 0 } },
            _ => new[,] { { 0, 0 }, { 2, 1 } },
        };
    }

    private (int column, int row) GetLocation()
    {
        return Type switch
        {
            1 or 3 => (2, 3),
            _ => (3, 2),
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
