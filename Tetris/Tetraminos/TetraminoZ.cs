using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Tetris;

internal class TetraminoZ : Tetramino
{
    public TetraminoZ(Rectangle background)
    {
        Color = Color.Red;
        Type = new Random().Next(0, 4);

        Initialize(Color, background);
    }

    public override void Rotate(Queue<Block>[] columns)
    {
        Block block = Blocks[0][0];
        Rectangle rectangle = block.Rectangle;

        switch (Type)
        {
            case 1:
                Type = 2;

                rectangle.X -= Block.Length;
                block.Column -= 1;
                block.Row += 1;
                break;
            case 2:
                Type = 3;

                block.Row -= 1;
                break;
            case 3:
                Type = 0;

                block.Row += 1;
                break;
            default:
                Type = 1;

                rectangle.X += Block.Length;
                block.Column += 1;
                block.Row -= 1;
                break;
        }

        Blocks[0][0].Rectangle = rectangle;
        Initialize(Color, Blocks[0][0]);
    }

    protected override void SetProperties()
    {
        switch (Type)
        {
            case 1:
            case 3:
                Columns = 2;
                Rows = 3;
                Skip = new[,] { { 0, 0 }, { 1, 2 } };
                break;
            case 2:
            default:
                Columns = 3;
                Rows = 2;
                Skip = new[,] { { 0, 1 }, { 2, 0 } };
                break;
        }
    }
}
