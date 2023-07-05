using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static Tetris.Constants;

namespace Tetris
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Rectangle background;
        Texture2D backgroundTexture;
        int currentColumn;

        Queue<Tuple<Rectangle, Texture2D>>[] columns;
        Tetramino currentEl;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // initialize queues for columns
            columns = new Queue<Tuple<Rectangle, Texture2D>>[TotalColumns]; // TODO: fix this
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

            currentEl = new StraightTetramino(GraphicsDevice, background);
            currentColumn = currentEl.Column;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            // collision detection
            int backgroundBottom = background.Y + background.Height;

            int columnBlocks = currentEl.Width / BlockDimension;
            int rowBlocks = currentEl.Height / BlockDimension;

            // find the maximum column height
            int maxColumnHeight = FindMaxColumnHeight(columnBlocks);

            // move the element down
            if (currentEl.Rectangle.Y < backgroundBottom - currentEl.Height - maxColumnHeight)
            {
                Rectangle temp = currentEl.Rectangle;
                temp.Y += 5;
                currentEl.Rectangle = temp;
            }
            else
            {
                int nextRow = 0;

                for (int i = currentColumn; i < currentColumn + columnBlocks; i++)
                {
                    // find the maximum row
                    if (columns[i].Count > nextRow)
                    {
                        nextRow = columns[i].Count;
                    }
                }

                for (int i = currentColumn; i < currentColumn + columnBlocks; i++) // TODO: fix this
                {
                    // add null blocks to the queue to level the rows
                    for (int j = 0; j < rowBlocks; j++)
                    {
                        while (columns[i].Count < nextRow)
                        {
                            columns[i].Enqueue(null);
                        }
                    }

                    // add blocks to the queue
                    for (int j = nextRow; j < nextRow + rowBlocks; j++)
                    {
                        Tuple<Rectangle, Texture2D> block = CreateBlock(i, j + 1);
                        columns[i].Enqueue(block);
                    }
                }

                // find full rows and remove them
                int minColumnHeight = FindMinColumnHeight();

                for (int i = 0; i < minColumnHeight; i++)
                {
                    bool isRowFull = CheckIfRowIsFull(i);
                    if (isRowFull) RemoveRow();
                }

                // when reaches the top border, the game ends
                if (columns[currentColumn].Count == TotalRows)
                {
                    throw new NotImplementedException(); // TODO: fix this
                }

                currentEl = new StraightTetramino(GraphicsDevice, background);
                currentColumn = currentEl.Column;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(backgroundTexture, background, Color.Red);

            foreach (var column in columns)
            {
                foreach (var queue in column)
                {
                    if (queue != null)
                    {
                        _spriteBatch.Draw(queue.Item2, queue.Item1, Color.White);
                    }
                }
            }

            _spriteBatch.Draw(currentEl.Texture, currentEl.Rectangle, Color.Orange);

            _spriteBatch.End();

            base.Draw(gameTime);
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

        private int FindMaxColumnHeight(int columnBlocks)
        {
            int max = columns[currentColumn].Count * BlockDimension;

            for (int i = currentColumn + 1; i < currentColumn + columnBlocks; i++)
            {
                int columnHeight = columns[i].Count * BlockDimension;

                if (columnHeight > max)
                {
                    max = columnHeight;
                }

            }

            return max;
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

        private void RemoveRow()
        {
            for (int j = 0; j < columns.Length; j++)
            {
                columns[j].Dequeue();

                // move the blocks down
                for (int k = 0; k < columns[j].Count; k++)
                {
                    // TPDP: fix this
                    var g = columns[j].Dequeue();
                    var p = g.Item1;
                    p.Y += BlockDimension;
                    columns[j].Enqueue(Tuple.Create(p, g.Item2));
                }
            }
        }

        private Tuple<Rectangle, Texture2D> CreateStraight(int column)
        {
            int width = BlockDimension * 4;
            int height = BlockDimension * 1;

            Rectangle square = new(
                background.X + BlockDimension * column,
                background.Y,
                width, height);
            Texture2D squareTexture = new(GraphicsDevice, 1, 1);
            squareTexture.SetData(new Color[] { Color.White });

            return Tuple.Create(square, squareTexture);
        }

        private Tuple<Rectangle, Texture2D> CreateSquare(int column)
        {
            int width = BlockDimension * 2;
            int height = BlockDimension * 2;

            Rectangle square = new(
                background.X + BlockDimension * column,
                background.Y,
                width, height);
            Texture2D squareTexture = new(GraphicsDevice, 1, 1);
            squareTexture.SetData(new Color[] { Color.White });

            return Tuple.Create(square, squareTexture);
        }

        private Tuple<Rectangle, Texture2D> CreateBlock(int column, int row)
        {
            Rectangle block = new(
                background.X + BlockDimension * column,
                background.Y + BackgroundHeight - BlockDimension * row,
                BlockDimension, BlockDimension);
            Texture2D blockTexture = new(GraphicsDevice, 1, 1);
            blockTexture.SetData(new Color[] { Color.White });

            return Tuple.Create(block, blockTexture);
        }

        private int GenerateColumnNumber()
        {
            return new Random().Next(0, TotalColumns - 3); // if element is square, totalColumns - 1
        }
    }
}

enum TetraminoType
{
    Straight,
    Square,
    Block
}