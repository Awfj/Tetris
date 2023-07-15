using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    internal class Block
    {
        public const int Length = 20;
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
                Length, Length);

            Texture = new(graphicsDevice, 1, 1);
            Texture.SetData(new Color[] { Color.White });
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color);
        }
    }
}
