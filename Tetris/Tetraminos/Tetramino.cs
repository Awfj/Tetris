using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Tetris.Tetraminos
{
    internal abstract class Tetramino
    {
        public List<List<Block>> Blocks { get; set; } = new();
        public Color Color { get; set; } = Color.White;
        protected int InitialColumn { get; } = 6;
        protected int InitialRow { get; } = 0;
        protected int Columns { get; set; }
        protected int Rows { get; set; }


        /*public int Width { get; set; }
        public int Height { get; set; }*/

        public abstract void Rotate(GraphicsDevice graphicsDevice);
    }
}
