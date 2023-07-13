using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    internal class Block
    {
        public int Width { get; set; } = Constants.BlockDimension;
        public int Height { get; set; } = Constants.BlockDimension;
        public int Column { get; set; }
        public int Row { get; set; }
        public Color Color { get; }
        public Rectangle Rectangle { get; set; }
        public Texture2D Texture { get; }

        public Block(int x, int y, int column, int row, Color color, GraphicsDevice graphicsDevice)
        {
            Column = column;
            Row = row;
            Color = color;

            Rectangle = new(
                x,
                y,
                Width, Height);

            Texture = new(graphicsDevice, 1, 1);
            Texture.SetData(new Color[] { Color.White });
        }
    }
}
