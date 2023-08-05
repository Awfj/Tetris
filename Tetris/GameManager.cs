using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

using static Tetris.Constants;
using static Tetris.Movement;

namespace Tetris;

public class GameManager
{
    private readonly Queue<Block>[] columns = new Queue<Block>[TotalColumns];
    private readonly Background background = new(Globals.GraphicsDevice);
    private ITetramino currentEl;

    private int keyDelay = Delay;
    private bool keyDelayActive = false;

    private KeyboardState keyboardState;

    public GameManager()
    {
        // initialize queues for columns
        columns = new Queue<Block>[TotalColumns]; // TODO: fix this
        for (int i = 0; i < columns.Length; i++)
        {
            columns[i] = new();
        }

        currentEl = RandomizeTetramino();
    }

    public void Update()
    {
        // TODO: Add your update logic here

        if (keyDelayActive)
        {
            keyDelay--;
            if (keyDelay <= 0)
            {
                keyDelay = Delay;
                keyDelayActive = false;
            }
        }

        // collision detection
        bool generateNext = MoveDown(currentEl, columns, background.rectangle);

        if (generateNext == false)
        {
            HandleInput();
        }
        else
        {
            // fill the block with block and generate a new element
            int currentColumn = currentEl.Blocks[0][0].Column;
            for (int i = currentColumn; i < currentColumn + currentEl.Blocks.Count; i++) // TODO: fix this
            {
                List<Block> blocksColumn = currentEl.Blocks[i - currentColumn];
                int backgroundBottomY = background.rectangle.Y + Background.Height;

                // if the current row already exists
                // for nesting
                if (TotalRows - currentEl.Blocks[i - currentColumn][0].Row <= columns[i].Count)
                {
                    // add the current element to the block
                    BlocksMatrix.NestBlocks(blocksColumn, ref columns[i], backgroundBottomY);
                }
                else
                {
                    int rowCount = currentEl.Blocks[i - currentColumn].Count;
                    int nextRow = currentEl.Blocks[i - currentColumn][^1].Row + 1;

                    // add null to the columns to level the rows
                    BlocksMatrix.LevelRows(columns[i], rowCount, nextRow);
                    BlocksMatrix.AddBlocks(blocksColumn, columns[i], rowCount, nextRow, backgroundBottomY);
                }
            }

            RowsRemoval.RemoveFullRows(columns);
            if (CheckIfGameFinished(currentColumn)) { }

            currentEl = RandomizeTetramino();
        }
    }

    public void Draw()
    {
        background.Draw();

        // draw the blocks
        foreach (var column in columns)
        {
            foreach (var block in column)
            {
                if (block != null)
                {
                    block.Draw();
                }
            }
        }

        // draw the current element
        foreach (var column in currentEl.Blocks)
        {
            foreach (var block in column)
            {
                block.Draw();
            }
        }
    }

    private bool CheckIfGameFinished(int currentColumn)
    {
        if (columns[currentColumn].Count == TotalRows)
        {
            // NOTE: The game didn't end once, when the element reached the top border
            throw new NotImplementedException(); // TODO: fix this
        }

        return false;
    }

    private void HandleInput()
    {
        keyboardState = Keyboard.GetState();

        Keys[] s = keyboardState.GetPressedKeys();

        foreach (Keys key in s)
        {
            switch (key)
            {
                case Keys.Left:
                    MoveHorizontally(currentEl, columns, Direction.Left, ref keyDelayActive);
                    break;
                case Keys.Right:
                    MoveHorizontally(currentEl, columns, Direction.Right, ref keyDelayActive);
                    break;
                case Keys.Up:
                    Rotate();
                    break;
            }
        }
    }

    private ITetramino RandomizeTetramino()
    {
        int type = new Random().Next(0, 7);
        //return TetraminoFactory.GetTetramino(background.rectangle, type);
        return new TetraminoI(background.rectangle);
    }

    private void Rotate() // TODO: restrict rotation when it collides with the border
    {
        if (keyDelayActive)
            return;

        currentEl.Rotate(columns);
        keyDelayActive = true;
    }
}
