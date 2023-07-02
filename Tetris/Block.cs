using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    internal class Block
    {
        public Block(int x, int y, int width, int height, GraphicsDevice graphicsDevice)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;

            Texture = new(graphicsDevice, 1, 1);
            Texture.SetData(new Color[] { Color.White });
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; }
        public int Height { get; }
        public Texture2D Texture { get; }
    }
}
