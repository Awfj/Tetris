using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Tetris;

internal interface ITetramino
{
    List<List<Block>> Blocks { get; set; }
    void Rotate(Queue<Block>[] columns);
}

internal abstract class Tetramino : ITetramino
{
    // first list is for columns, second - for rows
    public List<List<Block>> Blocks { get; set; } = new();
    public Color Color { get; set; } = Color.White;
    protected int InitialColumn { get; } = 6;
    protected int InitialRow { get; } = 0;
    protected int Columns { get; set; }
    protected int Rows { get; set; }
    protected int Type { get; set; }
    protected int[,] Skip { get; set; } = new int[0, 0];

    protected abstract void SetProperties();
    public abstract void Rotate(Queue<Block>[] columns);

    protected void Initialize(Color color, Rectangle background)
    {
        SetProperties();

        for (int i = 0; i < Columns; i++)
        {
            Blocks.Add(new List<Block>());

            for (int j = 0; j < Rows; j++)
            {
                if (ShouldInitializeBlock(Skip, i, j)) continue;

                Block newBlock = new(
                    background.X + Block.Length * (InitialColumn + i),
                        background.Y + Block.Length * (InitialRow + j),
                        InitialColumn + i,
                        InitialRow + j,
                        color,
                        Globals.GraphicsDevice);

                Blocks[i].Add(newBlock);
            }
        }
    }

    protected void Initialize(Color color, Block block)
    {
        Blocks = new();
        SetProperties();

        for (int i = 0; i < Columns; i++)
        {
            Blocks.Add(new List<Block>());

            for (int j = 0; j < Rows; j++)
            {
                if (ShouldInitializeBlock(Skip, i, j)) continue;

                Block newBlock = new(
                        block.Rectangle.X + Block.Length * i,
                        block.Rectangle.Y + Block.Length * j,
                        block.Column + i,
                        block.Row + j,
                        color,
                        Globals.GraphicsDevice);

                Blocks[i].Add(newBlock);
            }
        }
    }

    private static bool ShouldInitializeBlock(int[,] skip, int i, int j)
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
}
