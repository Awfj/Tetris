using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Tetris
{
    internal abstract class Tetramino
    {
        public Tetramino() {
            Column = 6;
            Row = 0;
            //Row = Height / Constants.BlockDimension;
        }

        public List<List<Block>> Blocks { get; set; } = new();
        public int Column { get; set; }
        public int Row { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Rectangle Rectangle { get; set; }
        public Texture2D Texture { get; set; }
    }
}
