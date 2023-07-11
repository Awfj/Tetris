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

        // starts from 0
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
            int columnBlocks = currentEl.Width / BlockDimension;
            int rowBlocks = currentEl.Height / BlockDimension;

            bool generateNext = MoveDown(columnBlocks, rowBlocks);

            if (generateNext == false)
            {
                HandleInput();
            }
            else
            {
                // fill the queue with blocks and generate a new element
                for (int i = currentColumn; i < currentColumn + columnBlocks; i++) // TODO: fix this
                {
                    // if the current row already exists
                    // for nesting
                    if (currentEl.Blocks[i - currentColumn][0].Row < columns[i].Count)
                    {
                        // add the current element to the queue

                        var temp = new Queue<Tuple<Rectangle, Texture2D>>();

                        int n = columns[i].Count;
                        for (int j = 0, b = currentEl.Blocks[i - currentColumn].Count - 1; j < n; j++)
                        {
                            var block = columns[i].Dequeue();

                            if (b >= 0 && temp.Count == currentEl.Blocks[i - currentColumn][b].Row - 1)
                            {
                                var bl = currentEl.Blocks[i - currentColumn][b];

                                temp.Enqueue(new Tuple<Rectangle, Texture2D>(bl.Rectangle, bl.Texture));
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
                        int nextRow = currentEl.Blocks[i - currentColumn][^1].Row - 1;

                        // add null to the queue to level the rows
                        for (int j = 0; j < rowCount; j++)
                        {
                            while (columns[i].Count < nextRow)
                            {
                                columns[i].Enqueue(null);
                            }
                        }

                        // add blocks to the queue
                        for (int j = nextRow; j < nextRow + rowCount; j++)
                        {
                            Tuple<Rectangle, Texture2D> block = CreateBlock(i, j + 1);
                            columns[i].Enqueue(block);
                        }
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

            foreach (var column in currentEl.Blocks)
            {
                foreach (var block in column)
                {
                    _spriteBatch.Draw(block.Texture, block.Rectangle, Color.Orange);
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private bool MoveDown(int columnBlocks, int rowBlocks)
        {
            foreach (var column in currentEl.Blocks)
            {
                foreach (var block in column)
                {
                    var s = block.Rectangle;
                    s.Y += speed;
                    block.Rectangle = s; // check if needs to move back to the previous position
                }
            }

            // check if the element is out of bounds
            foreach (var column in currentEl.Blocks)
            {
                Block lastBlock = column[^1]; // check if the last ablock is not null
                if (lastBlock.Rectangle.Y + lastBlock.Rectangle.Height > background.Y + background.Height)
                {
                    foreach (var columnv in currentEl.Blocks)
                    {
                        foreach (var block in columnv)
                        {
                            var s = block.Rectangle;
                            s.Y -= speed;
                            block.Rectangle = s; // check if needs to move back to the previous position
                        }
                    }
                    return true;
                }
            }

            // check if the element is colliding with another element
            for (int i = 0, currentColumn = currentEl.Column; i < currentEl.Blocks.Count; i++, currentColumn++)
            {
                // don't check if the current element is higher than the current column
                if (currentEl.Blocks[i][^1].Row - 1 > columns[currentColumn].Count)
                {
                    continue;
                }

                for (int j = currentEl.Blocks[i][^1].Row - 2; j >= 0; j--)
                {
                    if (columns[currentColumn].ElementAt(j) is null) continue;
                    var block = columns[currentColumn].ElementAt(j).Item1;

                    if (currentEl.Blocks[i][^1].Rectangle.Y + currentEl.Blocks[i][^1].Height > block.Y)
                    {
                        foreach (var columnv in currentEl.Blocks)
                        {
                            foreach (var blockw in columnv)
                            {
                                var s = blockw.Rectangle;
                                s.Y -= speed;
                                blockw.Rectangle = s; // check if needs to move back to the previous position
                            }
                        }
                        return true;
                    }
                }
            }

            int y = 0;

            for (int i = 0; i < currentEl.Blocks.Count; i++)
            {
                y = currentEl.Blocks[i][0].Rectangle.Y;
            }

            currentRow = (int)Math.Ceiling(((background.Y + background.Height) - y) / (double)20);
            currentEl.Row = currentRow;

            foreach (var column in currentEl.Blocks)
            {
                foreach (var block in column)
                {
                    block.Row = (int)Math.Ceiling(((background.Y + background.Height) - block.Rectangle.Y) / (double)20);
                }
            }

            return false;
        }

        private void Move(Direction direction)
        {
            if (keyDelayActive) return;

            //Tetramino temp = currentEl;

            foreach (var column in currentEl.Blocks)
            {
                foreach (var block in column)
                {
                    //if (ablock == null) continue;

                    var s = block.Rectangle;
                    //s.X += speed;
                    s.X = GetUpdatedX(direction, s);
                    block.Rectangle = s;
                }
            }

            if (direction == Direction.Left)
            {
                // check if the element is out of bounds
                foreach (var blocks in currentEl.Blocks[0])
                {
                    Block ablock = blocks; // check if the last ablock is not null
                    if (IsSideCollisionWithBorder(direction, ablock.Rectangle, background))
                    {
                        MoveBack(direction);
                        return;
                    }
                }
            }
            else if (direction == Direction.Right)
            {
                // check if the element is out of bounds
                foreach (var blocks in currentEl.Blocks[^1])
                {
                    Block ablock = blocks; // check if the last ablock is not null
                    if (IsSideCollisionWithBorder(direction, ablock.Rectangle, background))
                    {
                        MoveBack(direction);
                        return;
                    }
                }
            }

            

            // rows
            for (int i = 0; i < currentEl.Blocks.Count; i++)
            {
                // columns
                for (int j = 0; j < currentEl.Blocks[i].Count; j++)
                {
                    var curBlock = currentEl.Blocks[i][j];

                    if (curBlock.Row >= columns[curBlock.Column - 1].Count) continue;
                    var block = columns[curBlock.Column - 1].ElementAt(curBlock.Row - 1);
                    if (block is null) continue;

                    if (IsLeftCollisionWithBLock(curBlock.Rectangle, block.Item1))
                    {
                        MoveBack(direction);
                        return;
                    }
                }
            }

            currentEl.Column = GetAdjacentColumn(direction, currentEl);
            currentColumn = currentEl.Column;

            foreach (var column in currentEl.Blocks)
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

            keyDelayActive = true;
        }

        private void MoveBack(Direction direction)
        {
            if (direction == Direction.Left)
            {
                foreach (var column in currentEl.Blocks)
                {
                    foreach (var blockw in column)
                    {
                        //if (block == null) continue;

                        var s = blockw.Rectangle;
                        //s.X += speed;
                        s.X = GetUpdatedX(Direction.Right, s);
                        blockw.Rectangle = s;
                    }
                }
            }
            else
            {
                foreach (var column in currentEl.Blocks)
                {
                    foreach (var blockw in column)
                    {
                        //if (block == null) continue;

                        var s = blockw.Rectangle;
                        //s.X += speed;
                        s.X = GetUpdatedX(Direction.Left, s);
                        blockw.Rectangle = s;
                    }
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

        private static bool IsLeftCollisionWithBorder(Rectangle element, Rectangle background)
        {
            return element.X < background.X;
        }

        private static bool IsRightCollisionWithBorder(Rectangle element, Rectangle background)
        {
            return element.X + element.Width > background.X + background.Width;
        }

        private static int GetPrevColumn(Tetramino element)
        {
            return element.Column - 1;
        }

        private static int GetNextColumn(Tetramino element)
        {
            return element.Column + 1;
        }

        private static int GetColumnAfterElement(Tetramino element)
        {
            int elementHWidthInBlocks = element.Width / BlockDimension;
            return element.Column + elementHWidthInBlocks;
        }

        private static bool IsLeftCollisionWithBLock(Rectangle element, Rectangle block)
        {
            return element.X <= block.X + block.Width;
        }

        private static bool IsRightCollisionWithBlock(Rectangle element, Rectangle block)
        {
            return element.X + element.Width >= block.X;
        }

        private Dictionary<Direction, Tuple<
            Func<Rectangle, int>,
            Func<Rectangle, Rectangle, bool>, 
            Func<Tetramino, int>,
            Func<Rectangle, Rectangle, bool>,
            Func<Tetramino, int>
            >> _directionMap = new()
        {
            { Direction.Left, new Tuple<
                Func<Rectangle, int>,
                Func<Rectangle, Rectangle, bool>,
                Func<Tetramino, int>,
                Func<Rectangle, Rectangle, bool>,
                Func<Tetramino, int>
                >(GetUpdatedToLeftX, IsLeftCollisionWithBorder, GetPrevColumn, 
                IsLeftCollisionWithBLock, GetPrevColumn) },
            { Direction.Right, new Tuple<
                Func<Rectangle, int>,
                Func<Rectangle, Rectangle, bool>,
                Func<Tetramino, int>,
                Func<Rectangle, Rectangle, bool>,
                Func<Tetramino, int>
                >(GetUpdatedToRightX, IsRightCollisionWithBorder, GetColumnAfterElement, 
                IsRightCollisionWithBlock, GetNextColumn) }
        };

        private int GetUpdatedX(Direction direction, Rectangle element)
        {
            return _directionMap[direction].Item1.Invoke(element);
        }

        private bool IsSideCollisionWithBorder(Direction direction, Rectangle element, Rectangle background)
        {
            return _directionMap[direction].Item2.Invoke(element, background);
        }

        private int GetAdjacentColumnIndexToElement(Direction direction, Tetramino element)
        {
            return _directionMap[direction].Item3.Invoke(element);
        }

        private bool IsSideCollisionWithBlock(Direction direction, Rectangle element, Rectangle block)
        {
            return _directionMap[direction].Item4.Invoke(element, block);
        }

        private int GetAdjacentColumn(Direction direction, Tetramino element)
        {
            return _directionMap[direction].Item5.Invoke(element);
        }

        private enum Direction
        {
            Left,
            Right
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
                        Move(Direction.Left);
                        break;
                    case Keys.Right:
                        Move(Direction.Right);
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

        private void RemoveRow()
        {
            for (int j = 0; j < columns.Length; j++)
            {
                // remove filled row from every adjacentColumnIndex
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

            /*switch (n)
            {
                case 0:
                    return new StraightTetramino(GraphicsDevice, background);
                default:
                    return new SquareTetramino(GraphicsDevice, background);
            }*/

            //return new StraightTetramino(GraphicsDevice, background);
            return new SkewTetramino(GraphicsDevice, background);
        }
    }
}

enum TetraminoType
{
    Straight,
    Square,
    Block
}