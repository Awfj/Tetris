using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static Tetris.Constants;
using static Tetris.Movement;

namespace Tetris
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Rectangle background;
        Texture2D backgroundTexture;

        int keyDelay = Delay;
        bool keyDelayActive = false;

        private KeyboardState keyboardState;

        Queue<Block>[] columns;
        Tetramino currentEl;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // initialize queues for columns
            columns = new Queue<Block>[TotalColumns]; // TODO: fix this
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = new();
            }
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // make the background
            background = new(
                GraphicsDevice.Viewport.Width / 2 - BackgroundWidth / 2,
                GraphicsDevice.Viewport.Height / 2 - BackgroundHeight / 2,
                BackgroundWidth, BackgroundHeight);
            backgroundTexture = new(GraphicsDevice, 1, 1);
            backgroundTexture.SetData(new Color[] { Color.White });

            currentEl = RandomizeTetramino();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

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
            bool generateNext = MoveDown(currentEl, columns, background);

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
                    // if the current row already exists
                    // for nesting
                    if (TotalRows - currentEl.Blocks[i - currentColumn][0].Row <= columns[i].Count)
                    {
                        // add the current element to the block

                        var temp = new Queue<Block>();

                        int n = columns[i].Count;
                        for (int j = 0, b = currentEl.Blocks[i - currentColumn].Count - 1; j < n; j++)
                        {
                            var block = columns[i].Dequeue();

                            if (b >= 0 && temp.Count == TotalRows - currentEl.Blocks[i - currentColumn][b].Row - 1)
                            {
                                var bl = currentEl.Blocks[i - currentColumn][b];

                                Rectangle tempRectangle = bl.Rectangle;
                                tempRectangle.Y = background.Y + BackgroundHeight - BlockDimension * (TotalRows - bl.Row);

                                bl.Rectangle = tempRectangle;

                                temp.Enqueue(bl);
                                b--;
                            }
                            else
                            {
                                temp.Enqueue(block);
                            }
                        }

                        columns[i] = temp;
                    }
                    else
                    {
                        int rowCount = currentEl.Blocks[i - currentColumn].Count;
                        int nextRow = currentEl.Blocks[i - currentColumn][^1].Row + 1;

                        // add null to the block to level the rows
                        for (int j = 0; j < rowCount; j++)
                        {
                            while (columns[i].Count < TotalRows - nextRow)
                            {
                                columns[i].Enqueue(null);
                            }
                        }

                        // add block to the block
                        for (int j = nextRow + rowCount - 1; j >= nextRow; j--)
                        {
                            Block currentBlock = currentEl.Blocks[i - currentColumn][j - nextRow];

                            Rectangle tempRectangle = currentBlock.Rectangle;
                            tempRectangle.Y = background.Y + BackgroundHeight - BlockDimension * (TotalRows - currentBlock.Row);

                            currentBlock.Rectangle = tempRectangle;
                            columns[i].Enqueue(currentBlock);
                        }
                    }
                }

                // find full rows and remove them
                int minColumnHeight = FindMinColumnHeight();
                int count = 0;

                for (int i = 0; i < minColumnHeight; i++)
                {
                    bool isRowFull = CheckIfRowIsFull(i - count);

                    if (isRowFull) // TODO: check if works correctly
                    {
                        RemoveRow();
                        count++;
                    }
                }

                // when reaches the top border, the game ends
                if (columns[currentColumn].Count == TotalRows)
                {
                    // NOTE: The game didn't end once, when the element reached the top border
                    throw new NotImplementedException(); // TODO: fix this
                }

                currentEl = RandomizeTetramino();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(backgroundTexture, background, Color.Pink);

            // draw the blocks
            foreach (var column in columns)
            {
                foreach (var block in column)
                {
                    if (block != null)
                    {
                        _spriteBatch.Draw(block.Texture, block.Rectangle, block.Color);
                    }
                }
            }

            // draw the current element
            foreach (var column in currentEl.Blocks)
            {
                foreach (var block in column)
                {
                    _spriteBatch.Draw(block.Texture, block.Rectangle, currentEl.Color);
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
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

        private int FindMinColumnHeight()
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

        private bool CheckIfRowIsFull(int row)
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

        private void RemoveRow() // TODO: check if workds correctly
        {
            for (int i = 0; i < columns.Length; i++)
            {
                // remove filled row from every adjacentColumnIndex
                columns[i].Dequeue();

                for (int j = 0; j < columns[i].Count; j++)
                {
                    if (columns[i].ElementAt(0) is not null)
                    {
                        Block block = columns[i].ElementAt(j);
                        Rectangle rect = block.Rectangle;
                        rect.Y += BlockDimension;
                        block.Rectangle = rect;
                    }
                }
            }
        }

        private Tetramino RandomizeTetramino()
        {
            int type = new Random().Next(0, 3);

            switch (type)
            {
                case 0:
                    return new TetraminoI(GraphicsDevice, background);
                case 1:
                    return new TetraminoZ(GraphicsDevice, background);
                default:
                    return new TetraminoO(GraphicsDevice, background);
            }
        }

        private void Rotate() // TODO: restrict rotation when it collides with the border
        {
            if (keyDelayActive) return;

            currentEl.Rotate(GraphicsDevice);
            keyDelayActive = true;
        }
    }
}