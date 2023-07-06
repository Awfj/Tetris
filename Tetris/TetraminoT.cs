using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Tetris
{
    internal class TetraminoT : Tetramino
    {
        public TetraminoT(GraphicsDevice graphicsDevice, Rectangle background)
        {
            RandomizeOrientation();

            Rectangle = new Rectangle(
                background.X + Constants.BlockDimension * Column,
                background.Y,
                Width, Height);

            Texture = new(graphicsDevice, 1, 1);
            Texture.SetData(new Color[] { Color.White });
        }

        private void RandomizeOrientation()
        {
            //int n = new Random().Next(0, 2);
            int n = 0; // TODO: remove this
            switch (n)
            {
                case 0:
                    Column = new Random().Next(0, Constants.TotalColumns - 3);
                    Width = Constants.BlockDimension * 4;
                    Height = Constants.BlockDimension;
                    break;
                default:
                    Column = new Random().Next(0, Constants.TotalColumns);
                    Width = Constants.BlockDimension;
                    Height = Constants.BlockDimension * 4;
                    break;
            }
        }
    }
}
