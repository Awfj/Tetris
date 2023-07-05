using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    internal abstract class Tetramino
    {
        public int Column { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Rectangle Rectangle { get; set; }
        public Texture2D Texture { get; set; }
    }
}
