using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    internal class Block
    {
        public Block(int x, int y, int column, int row, GraphicsDevice graphicsDevice)
        {
            /*X = x;
            Y = y;
            Width = Constants.BlockDimension;
            Height = Constants.BlockDimension;*/
            Column = column;
            Row = row;

            Width = Constants.BlockDimension;
            Height = Constants.BlockDimension;

            Rectangle = new(
                x,
                y,
                Width, Height);

            Texture = new(graphicsDevice, 1, 1);
            Texture.SetData(new Color[] { Color.White });
        }

        /*public int X { get; set; }
        public int Y { get; set; }*/
        public int Width { get; set; }
        public int Height { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public Rectangle Rectangle { get; set; }
        public Texture2D Texture { get; }
    }
}
