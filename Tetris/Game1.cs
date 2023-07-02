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

        List<Tuple<Rectangle, Texture2D>> elements;

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
            elements = new();

            // TODO: use this.Content to load your game content here
            int backgroundWidth = 300;
            int backgroundHeight = 400;
            background = new(
                GraphicsDevice.Viewport.Width / 2 - backgroundWidth / 2,
                GraphicsDevice.Viewport.Height / 2 - backgroundHeight / 2,
                backgroundWidth, backgroundHeight);
            backgroundTexture = new(GraphicsDevice, 1, 1);
            backgroundTexture.SetData(new Color[] { Color.White });

            int currentColumn = GenerateColumnNumber();
            Tuple<Rectangle, Texture2D> block = CreateBlock(currentColumn);
            var (element, texture) = block;

            //elements.Add(Tuple.Create(block.Item1, block.Item2));
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
            if (currentElement.Y < background.Y + background.Height - currentElement.Height)
            {
                currentElement.Y += 1;
            }
            else
            {
                elements.Add(Tuple.Create(currentElement, currentElementTexture));

                int currentColumn = GenerateColumnNumber();
                Tuple<Rectangle, Texture2D> block = CreateBlock(currentColumn);
                var (element, texture) = block;

                currentElement = element;
                currentElementTexture = texture;
            }

            // randomly choose a column
            //int currentColumn = GenerateColumnNumber();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(backgroundTexture, background, Color.Red);

            foreach (var element in elements)
            {
                _spriteBatch.Draw(element.Item2, element.Item1, Color.White);
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