using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Rectangle background;
        Texture2D backgroundTexture;
        int columns;
        int rows;
        int currentColumn;

        const int BlockDimension = 20;
        const int BackgroundWidth = 300;
        const int BackgroundHeight = 400;

        Queue<Tuple<Rectangle, Texture2D>>[] blocks;
        Tuple<Rectangle, Texture2D> currentEl;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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

            columns = BackgroundWidth / BlockDimension;
            rows = BackgroundHeight / BlockDimension;

            blocks = new Queue<Tuple<Rectangle, Texture2D>>[columns]; // TODO: fix this
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = new();
            }


            background = new(
                GraphicsDevice.Viewport.Width / 2 - BackgroundWidth / 2,
                GraphicsDevice.Viewport.Height / 2 - BackgroundHeight / 2,
                BackgroundWidth, BackgroundHeight);
            backgroundTexture = new(GraphicsDevice, 1, 1);
            backgroundTexture.SetData(new Color[] { Color.White });

            currentColumn = GenerateColumnNumber();
            Tuple<Rectangle, Texture2D> element = CreateSquare(currentColumn);

            currentEl = element;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var (currentElement, currentElementTexture) = currentEl;
            
            // collision detection
            int backgroundBottom = background.Y + background.Height;
            int columnHeight = blocks[currentColumn].Count * BlockDimension;
            int columnHeight2 = blocks[currentColumn + 1 == 15 ? 14 : currentColumn + 1].Count * BlockDimension;

            if (currentElement.Y < backgroundBottom - currentElement.Height - columnHeight &&
                currentElement.Y < backgroundBottom - currentElement.Height - columnHeight2) // TPDP: fix this
            {
                // move the element down
                //int columnHeight2 = blocks[currentColumn + 1].Count * BlockDimension;
                /*if (currentColumn <= 14 && currentElement.Y < backgroundBottom - currentElement.Height - columnHeight2)
                {
                    currentElement.Y += 5;
                    currentEl = Tuple.Create(currentElement, currentElementTexture);
                }*/
                currentElement.Y += 5;
                currentEl = Tuple.Create(currentElement, currentElementTexture);
            }
            else
            {
                int columnBlocks = currentElement.Width / BlockDimension;
                int rowBlocks = currentElement.Height / BlockDimension;


                int nextRow = 0;
                int f = currentColumn == 14 ? 13 : currentColumn;

                for (int i = currentColumn; i < f + columnBlocks; i++)
                {
                    // find the maximum row
                    if (blocks[i].Count > nextRow)
                    {
                        nextRow = blocks[i].Count;
                    }
                }

                for (int i = currentColumn; i < f + columnBlocks; i++) // TODO: fix this
                {


                    // add null blocks to the queue to level the rows
                    for (int j = 0; j < rowBlocks; j++)
                    {
                        while (blocks[i].Count < nextRow)
                        {
                            blocks[i].Enqueue(null);
                        }
                    }

                    //nextRow++;

                    // add blocks to the queue
                    for (int j = nextRow; j < nextRow + rowBlocks; j++)
                    {
                        Tuple<Rectangle, Texture2D> block = CreateBlock(i, j + 1, currentElement.Height);
                        blocks[i].Enqueue(block);
                    }

                    //nextRow = 0;
                }



                //blocks[currentColumn].Enqueue(Tuple.Create(currentElement, currentElementTexture));

                // find the minimum row
                int minRow = blocks[0].Count;
                for (int i = 1; i < blocks.Length; i++)
                {
                    if (blocks[i].Count < minRow)
                    {
                        minRow = blocks[i].Count;
                    }
                }

                for (int i = 0; i < minRow; i++)
                {
                    // check if there is a full row
                    bool isRowFull = false;

                    for (int j = 0; j < blocks.Length; j++)
                    {
                        if (blocks[j].ElementAt(i) == null)
                        {
                            break;
                        }

                        if (j == blocks.Length - 1)
                        {
                            isRowFull = true;
                        }
                    }

                    /*bool isRowFull = false;
                    for (int j = 0; j < blocks.Length; j++)
                    {
                        if (j >= blocks.Length - 1)
                        {
                            isRowFull = true;
                        }

                        if (blocks[i] != null)
                        {
                            if (j >= blocks.Length - 1)
                            {
                                isRowFull = true;
                            }
                        }
                        else break;
                    }*/

                    // remove the row
                    if (isRowFull)
                    {
                        for (int j = 0; j < blocks.Length; j++)
                        {
                            blocks[j].Dequeue();

                            // move the blocks down
                            for (int k = 0; k < blocks[j].Count; k++)
                            {
                                // TPDP: fix this
                                var g = blocks[j].Dequeue();
                                var p = g.Item1;
                                p.Y += BlockDimension;
                                blocks[j].Enqueue(Tuple.Create(p, g.Item2));
                            }
                        }
                    }
                }

                // when reaches the top border, the game ends
                if (blocks[currentColumn].Count == rows)
                {
                    throw new NotImplementedException(); // TODO: fix this
                }

                currentColumn = GenerateColumnNumber();
                Tuple<Rectangle, Texture2D> element = CreateSquare(currentColumn);

                currentEl = element;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(backgroundTexture, background, Color.Red);

            foreach (var column in blocks)
            {
                foreach (var queue in column)
                {
                    if (queue != null)
                    {
                        _spriteBatch.Draw(queue.Item2, queue.Item1, Color.White);
                    }
                }
            }
            _spriteBatch.Draw(currentEl.Item2, currentEl.Item1, Color.Orange);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private Tuple<Rectangle, Texture2D> CreateSquare(int column)
        {
            int squareDimension = BlockDimension * 2;
            if (column == columns - 1) column--;

            Rectangle square = new(
                background.X + BlockDimension * column,
                background.Y,
                squareDimension, squareDimension);
            Texture2D squareTexture = new(GraphicsDevice, 1, 1); ;
            squareTexture.SetData(new Color[] { Color.White });

            return Tuple.Create(square, squareTexture);
        }

        private Tuple<Rectangle, Texture2D> CreateBlock(int column, int row, int elementHeight)
        {
            Rectangle block = new(
                background.X + BlockDimension * column,
                background.Y + BackgroundHeight - BlockDimension * row,
                BlockDimension, BlockDimension);
            Texture2D blockTexture = new(GraphicsDevice, 1, 1); ;
            blockTexture.SetData(new Color[] { Color.White });

            return Tuple.Create(block, blockTexture);
        }

        private int GenerateColumnNumber()
        {
            return new Random().Next(0, columns);
        }
    }
}