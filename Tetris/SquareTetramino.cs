using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;

namespace Tetris
{
    internal class SquareTetramino : Tetramino
    {
        public SquareTetramino(GraphicsDevice graphicsDevice, Rectangle background)
        {
            RandomizeOrientation();
            Column = 7;

            Rectangle = new Rectangle(
                background.X + Constants.BlockDimension * Column,
                background.Y,
                Width, Height);
            Texture = new(graphicsDevice, 1, 1);
            Texture.SetData(new Color[] { Color.White });
        }

        private void RandomizeOrientation()
        {
            Column = new Random().Next(0, Constants.TotalColumns - 1);
            Width = Constants.BlockDimension * 2;
            Height = Constants.BlockDimension * 2;
        }
    }
}
