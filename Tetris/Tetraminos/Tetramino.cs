using System.Collections.Generic;

namespace Tetris.Tetraminos
{
    internal abstract class Tetramino
    {
        public List<List<Block>> Blocks { get; set; } = new();
        public int Column { get; set; } = 6;
        public int Row { get; set; } = 0;
        public int Columns { get; set; }
        public int Rows { get; set; }


        /*public int Width { get; set; }
        public int Height { get; set; }*/

        public abstract void Rotate();
    }
}
