using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Tetris.Tetraminos;
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
        //int currentColumn;
        //int currentRow;

        int speed = 2;
        int TotalRows = 20;

        int keyDelay = Delay;
        bool keyDelayActive = false;

        private KeyboardState keyboardState;

        Queue<Block>[] columns;
        Rectangle[] rectangles;
        Tetramino currentEl;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            rectangles = new Rectangle[2];
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
            //currentEl = new ZHorizontalTetramino(GraphicsDevice, background);
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
            //int columnBlocks = currentEl.Width / BlockDimension;
            //int rowBlocks = currentEl.Height / BlockDimension;

            bool generateNext = MoveDown();

            if (generateNext == false)
            {
                HandleInput();
            }
            else
            {
                // fill the block with block and generate a new element
                int currentColumn = currentEl.Blocks[0][0].Column;
                //for (int i = currentColumn; i < currentColumn + currentEl.Columns; i++) // TODO: fix this
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
                                /*Rectangle tempRectangle = block.Rectangle;
                                tempRectangle.Y = background.Y + BackgroundHeight - BlockDimension * (TotalRows - block.InitialRow);

                                block.Rectangle = tempRectangle;*/

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
                            /*int column = i;
                            int row = j + 1;
                            Block block = new(
                                background.X + BlockDimension * column, 
                                background.Y + BackgroundHeight - BlockDimension * row,
                                column, row, GraphicsDevice);*/
                            //columns[i].Enqueue(block);
                            Block currentBlock = currentEl.Blocks[i - currentColumn][j - nextRow];

                            Rectangle tempRectangle = currentBlock.Rectangle;
                            tempRectangle.Y = background.Y + BackgroundHeight - BlockDimension * (TotalRows - currentBlock.Row);

                            currentBlock.Rectangle = tempRectangle;

                        /*    background.X + Constants.BlockDimension * (InitialColumn + i),
                    background.Y + Constants.BlockDimension * (InitialRow + j)*/

                            columns[i].Enqueue(currentBlock);
                        }
                    }

                }

                // find full rows and remove them
                /*int minColumnHeight = FindMinColumnHeight();
                int count = 0;

                for (int i = 0; i < minColumnHeight; i++)
                {
                    bool isRowFull = CheckIfRowIsFull(i - count);
                    if (isRowFull)
                    {
                        RemoveRow();
                        count++;
                    }
                }*/

                // when reaches the top border, the game ends
                if (columns[currentColumn].Count == TotalRows)
                {
                    // NOTE: The game didn't end once, when the element reached the top border
                    throw new NotImplementedException(); // TODO: fix this
                }

                currentEl = RandomizeTetramino();
                //currentEl = new ZHorizontalTetramino(GraphicsDevice, currentEl.Blocks[0][0].Rectangle.X, currentEl.Blocks[0][0].Rectangle.Y);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(backgroundTexture, background, Color.Pink);

            foreach (var column in columns)
            {
                foreach (var block in column)
                {
                    if (block != null)
                    {
                        _spriteBatch.Draw(block.Texture, block.Rectangle, currentEl.Color);
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

        private bool MoveDown()
        {
            // check if the element is at the bottom
            foreach (var column in currentEl.Blocks)
            {
                if (column[^1].Row >= 19) return true;
            }

            // check if the element collides with another element
            foreach (var column in currentEl.Blocks)
            {
                foreach (var block in column)
                {
                    int nextRow = TotalRows - block.Row - 2;
                    if (columns[block.Column].Count <= nextRow) continue;
                    if (columns[block.Column].ElementAt(nextRow) != null)
                    {
                        return true;
                    }
                }
            }

            // change the position of the element
            foreach (var column in currentEl.Blocks)
            {
                foreach (var block in column)
                {
                    var s = block.Rectangle;
                    s.Y += speed;
                    block.Rectangle = s; // if speed < TotalRows
                }
            }

            // change the row of the element
            foreach (var column in currentEl.Blocks)
            {
                foreach (var block in column)
                {
                    block.Row = TotalRows - (int)Math.Ceiling(((background.Y + background.Height) - block.Rectangle.Y) / (double)20);
                }
            }

            return false;
        }

        private void Move(Direction direction)
        {
            if (keyDelayActive) return;

            // check if the element is out of bounds
            if (direction == Direction.Left)
            {
                foreach (var block in currentEl.Blocks[0])
                {
                    if (block.Column <= 0) return;

                }
            }
            else if (direction == Direction.Right)
            {
                foreach (var block in currentEl.Blocks[^1])
                {
                    if (block.Column >= 14) return; // 14 is last column index
                }
            }

            // check if the element collides with another element
            if (direction == Direction.Left)
            {
                foreach (var column in currentEl.Blocks)
                {
                    foreach (var block in column)
                    {
                        //int nextRow = TotalRows - block.InitialRow - 2;
                        int nextColumn = block.Column - 1;
                        int row = TotalRows - block.Row - 1;

                        //if (columns[block.InitialColumn].Count <= nextRow) continue;
                        if (row >= columns[nextColumn].Count) continue;
                        if (columns[nextColumn].ElementAt(TotalRows - block.Row - 1) != null)
                        {
                            return;
                        }
                    }
                }
            }
            else if (direction == Direction.Right)
            {
                foreach (var column in currentEl.Blocks)
                {
                    foreach (var block in column)
                    {
                        //int nextRow = TotalRows - block.InitialRow - 2;
                        int nextColumn = block.Column + 1;
                        int row = TotalRows - block.Row - 1;

                        //if (columns[block.InitialColumn].Count <= nextRow) continue;
                        if (row >= columns[nextColumn].Count) continue;
                        if (columns[nextColumn].ElementAt(TotalRows - block.Row - 1) != null)
                        {
                            return;
                        }
                    }
                }
            }

            // change the position of the element
            foreach (var column in currentEl.Blocks)
            {
                foreach (var block in column)
                {
                    //if (ablock == null) continue;

                    var s = block.Rectangle;

                    if (direction == Direction.Right)
                    {
                        s.X = GetUpdatedToRightX(s);
                    }
                    else if (direction == Direction.Left)
                        s.X = GetUpdatedToLeftX(s);


                    block.Rectangle = s;
                }
            }

            // change the column of the element
            //currentEl.InitialColumn = GetAdjacentColumn(direction, currentEl);

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

        private static int GetUpdatedToLeftX(Rectangle element)
        {
            return element.X - 20;
        }

        private static int GetUpdatedToRightX(Rectangle element)
        {
            return element.X + 20;
        }

        /*private static bool IsLeftCollisionWithBorder(Rectangle element, Rectangle background)
        {
            return element.X < background.X;
        }

        private static bool IsRightCollisionWithBorder(Rectangle element, Rectangle background)
        {
            return element.X + element.Width > background.X + background.Width;
        }

        private static int GetPrevColumn(Tetramino element)
        {
            return element.InitialColumn - 1;
        }

        private static int GetNextColumn(Tetramino element)
        {
            return element.InitialColumn + 1;
        }

        private static int GetColumnAfterElement(Tetramino element)
        {
            //int elementHWidthInBlocks = element.Width / BlockDimension;
            int elementHWidthInBlocks = 0; // TODO: remove this
            return element.InitialColumn + elementHWidthInBlocks;
        }

        private static bool IsLeftCollisionWithBLock(Rectangle element, Rectangle block)
        {
            return element.X <= block.X + block.Width;
        }

        private static bool IsRightCollisionWithBlock(Rectangle element, Rectangle block)
        {
            return element.X + element.Width >= block.X;
        }*/

        /*private Dictionary<Direction, Tuple<
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
        }*/

        /*private bool IsSideCollisionWithBorder(Direction direction, Rectangle element, Rectangle background)
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
        }*/

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

        private void RemoveRow()
        {
            for (int j = 0; j < columns.Length; j++)
            {
                // remove filled row from every adjacentColumnIndex
                columns[j].Dequeue();

                // move the block down
                for (int k = 0; k < columns[j].Count; k++)
                {

                    if (columns[j].ElementAt(0) is not null)
                    {
                        // TPDP: fix this
                        var g = columns[j].Dequeue();
                        var p = g.Rectangle;
                        p.Y += BlockDimension;
                        columns[j].Enqueue(g); // TPDP: fix this
                    }
                }
            }
        }

        /*private int GenerateColumnNumber()
        {
            return new Random().Next(0, TotalColumns - 3); // if element is square, totalColumns - 1
        }*/

        private Tetramino RandomizeTetramino()
        {
            int type = new Random().Next(0, 3);

            switch (n)
            {
                case 0:
                    return new StraightTetramino(GraphicsDevice, background);
                default:
                    return new TetraminoO(GraphicsDevice, background);
            }

            //return new StraightTetramino(GraphicsDevice, background);
            //return new SkewTetramino(GraphicsDevice, background);
            return new TetraminoI(GraphicsDevice, background, 1);
        }

        private void Rotate()
        {
            if (keyDelayActive) return;

            currentEl.Rotate(GraphicsDevice);
            keyDelayActive = true;
        }
    }
}

enum TetraminoType
{
    Straight,
    Square,
    Block
}