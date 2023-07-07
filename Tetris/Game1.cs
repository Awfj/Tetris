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
        int currentRow;
        int speed = 2;

        int keyDelay = Delay;
        bool keyDelayActive = false;

        private KeyboardState keyboardState;

        Queue<Tuple<Rectangle, Texture2D>>[] columns;
        Rectangle[] rectangles;
        Tetramino currentEl;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            rectangles = new Rectangle[2];
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

            currentEl = RandomizeTetramino();
            currentColumn = currentEl.Column;
            currentRow = currentEl.Row;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
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
            int columnBlocks = currentEl.Width / BlockDimension;
            int rowBlocks = currentEl.Height / BlockDimension;

            bool generateNext = MoveDown(columnBlocks);

            if (generateNext == false)
            {
                HandleInput();
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
                int count = 0;

                for (int i = 0; i < minColumnHeight; i++)
                {
                    bool isRowFull = CheckIfRowIsFull(i - count);
                    if (isRowFull)
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
                currentColumn = currentEl.Column;
                currentRow = currentEl.Row;
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

        private bool HandleCollision(string direction, int columnBlocks)
        {
            Rectangle temp = currentEl.Rectangle;
            //Rectangle temp2 = currentEl.Rectangle;



            switch (direction)
            {
                case "left":
                    temp.X -= speed;
                    //temp2.X -= speed;
                    break;
                case "right":
                    temp.X += speed;
                    //temp2.X += speed;
                    break;
                case "down":
                    temp.Y += speed;
                    //temp2.Y += speed;
                    break;
            }

            if (direction == "left")
            {
                if (temp.X < background.X) return true;
            }

            if (direction == "down")
            {
                //currentEl.Rectangle.Y < backgroundBottom - currentEl.Height - maxColumnHeight
                int backgroundBottom = background.Y + background.Height;
                int maxColumnHeight = FindMaxColumnHeight(columnBlocks);
                if (currentEl.Rectangle.Y >= backgroundBottom - currentEl.Height - maxColumnHeight)
                {
                    return false;
                }
            }
            else
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    for (int j = 0; j < columns[i].Count; j++)
                    {
                        if (columns[i].ElementAt(j) is null) continue;
                        var s = columns[i].ElementAt(j).Item1;


                        switch (direction)
                        {
                            case "left":
                                //  || temp2.X <= s.X + s.Width
                                //if (temp.X < s.X + s.Width)
                                if (currentEl.Column - 1 == i && s.Y >= temp.Y && s.Y <= temp.Y + temp.Height)
                                {
                                    return false;
                                }
                                break;
                            case "right":
                                //  || temp2.X + temp2.Width >= s.X
                                if (temp.X + temp.Width > s.X)
                                {
                                    //return false;
                                }
                                break;
                        }

                    }
                }


            }
            currentEl.Rectangle = temp;
            return true;
        }

        private bool MoveDown(int columnBlocks)
        {
            Rectangle temp = currentEl.Rectangle;
            temp.Y += speed;

            int backgroundBottom = background.Y + background.Height;
            int maxColumnHeight = FindMaxColumnHeight(columnBlocks);
            if (currentEl.Rectangle.Y >= backgroundBottom - currentEl.Height - maxColumnHeight || currentEl.Rectangle.Y < background.Y)
            {
                return true;
            }

            currentEl.Rectangle = temp;
            currentRow = (currentEl.Rectangle.Y - background.Y) % speed;
            return false;
        }

        private void MoveLeft()
        {
            if (keyDelayActive) return;

            Rectangle temp = currentEl.Rectangle;
            temp.X -= 20;

            if (temp.X < background.X) return;

            for (int i = 0; i < columns.Length; i++)
            {
                for (int j = 0; j < columns[i].Count; j++)
                {
                    if (columns[i].ElementAt(j) is null) continue;
                    var s = columns[i].ElementAt(j).Item1;


                    if (currentEl.Column - 1 == i && s.Y >= temp.Y && s.Y <= temp.Y + temp.Height)
                    {
                        return;
                    }

                }
            }

            currentEl.Rectangle = temp;
            currentEl.Column--;
            currentColumn = currentEl.Column;

            keyDelayActive = true;
        }

        private void MoveRight()
        {
            if (keyDelayActive) return;

            Rectangle temp = currentEl.Rectangle;
            temp.X += 20;

            if (temp.X + temp.Width > background.X + background.Width) return;

            for (int i = 0; i < columns.Length; i++)
            {
                for (int j = 0; j < columns[i].Count; j++)
                {
                    if (columns[i].ElementAt(j) is null) continue;
                    var s = columns[i].ElementAt(j).Item1;

                    if (currentEl.Column + 1 == i && s.Y >= temp.Y && s.Y <= temp.Y + temp.Height)
                    {
                        return;
                    }
                }
            }

            currentEl.Rectangle = temp;
            currentEl.Column++;
            currentColumn = currentEl.Column;

            keyDelayActive = true;
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
                        MoveLeft();
                        break;
                    case Keys.Right:
                        MoveRight();
                        break;
                }
            }

            /*if ()
            {
                MoveLeft(); // TODO: fix this
            }*/
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
                // remove filled row from every column
                columns[j].Dequeue();

                // move the blocks down
                for (int k = 0; k < columns[j].Count; k++)
                {

                    if (columns[j].ElementAt(0) is not null)
                    {
                        // TPDP: fix this
                        var g = columns[j].Dequeue();
                        var p = g.Item1;
                        p.Y += BlockDimension;
                        columns[j].Enqueue(Tuple.Create(p, g.Item2));
                    }                    
                }
            }
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

        /*private int GenerateColumnNumber()
        {
            return new Random().Next(0, TotalColumns - 3); // if element is square, totalColumns - 1
        }*/

        private Tetramino RandomizeTetramino()
        {
            int n = new Random().Next(0, 2);

            switch (n)
            {
                case 0:
                    return new StraightTetramino(GraphicsDevice, background);
                default:
                    return new SquareTetramino(GraphicsDevice, background);
            }
        }
    }
}

enum TetraminoType
{
    Straight,
    Square,
    Block
}