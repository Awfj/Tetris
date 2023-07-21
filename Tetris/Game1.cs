using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Taskbar;
using static Tetris.Constants;
using static Tetris.Movement;

namespace Tetris
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Background background;
        private int keyDelay = Delay;
        private bool keyDelayActive = false;

        private KeyboardState keyboardState;
        private Queue<Block>[] columns;
        private Tetramino currentEl;

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

            background = new Background(GraphicsDevice);
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

            base.Update(gameTime);
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            background.Draw(_spriteBatch);

            // draw the blocks
            foreach (var column in columns)
            {
                foreach (var block in column)
                {
                    if (block != null)
                    {
                        block.Draw(_spriteBatch);
                    }
                }
            }

            // draw the current element
            foreach (var column in currentEl.Blocks)
            {
                foreach (var block in column)
                {
                    block.Draw(_spriteBatch);
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

        private Tetramino RandomizeTetramino()
        {
            int type = new Random().Next(0, 7);
            return TetraminoFactory.GetTetramino(GraphicsDevice, background.rectangle, type);
        }

        private void Rotate() // TODO: restrict rotation when it collides with the border
        {
            if (keyDelayActive) return;

            currentEl.Rotate(GraphicsDevice);
            keyDelayActive = true;
        }
    }
}