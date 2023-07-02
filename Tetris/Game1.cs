using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Tetris
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Rectangle background;
        Rectangle currentElement;

        Texture2D backgroundTexture;
        Texture2D currentElementTexture;
        int columns;
        int rows;
        int currentColumn;

        Queue<Tuple<Rectangle, Texture2D>>[] blocks;
        //List<Queue<Tuple<Rectangle, Texture2D>>> elements;

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

            blocks = new Queue<Tuple<Rectangle, Texture2D>>[6]; // TODO: fix this
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i] = new();
            }

            int backgroundWidth = 300;
            int backgroundHeight = 400;
            background = new(
                GraphicsDevice.Viewport.Width / 2 - backgroundWidth / 2,
                GraphicsDevice.Viewport.Height / 2 - backgroundHeight / 2,
                backgroundWidth, backgroundHeight);
            backgroundTexture = new(GraphicsDevice, 1, 1);
            backgroundTexture.SetData(new Color[] { Color.White });

            currentColumn = GenerateColumnNumber();
            Tuple<Rectangle, Texture2D> block = CreateBlock(currentColumn);
            var (element, texture) = block;

            currentElement = element;
            currentElementTexture = texture;

            columns = background.Width / element.Width;
            rows = background.Height / element.Height;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (currentElement.Y < background.Y + background.Height - currentElement.Height - (blocks[currentColumn].Count * 50)) // TPDP: fix this
            {
                currentElement.Y += 10;
            }
            else
            {
                blocks[currentColumn].Enqueue(Tuple.Create(currentElement, currentElementTexture));

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
                        if (blocks[i] != null)
                        {
                            if (j >= blocks.Length - 1)
                            {
                                isRowFull = true;
                            }
                        }
                        else break;
                    }

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
                                p.Y += 50;
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
                Tuple<Rectangle, Texture2D> block = CreateBlock(currentColumn);
                var (element, texture) = block;

                currentElement = element;
                currentElementTexture = texture;
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
                    _spriteBatch.Draw(queue.Item2, queue.Item1, Color.White);
                }
            }
            _spriteBatch.Draw(currentElementTexture, currentElement, Color.Orange);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private Tuple<Rectangle, Texture2D> CreateBlock(int column)
        {
            int blockWidth = 50;
            int blockHeight = 50;

            Rectangle block = new(
                background.X + blockWidth * column,
                background.Y,
                blockWidth, blockHeight);
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