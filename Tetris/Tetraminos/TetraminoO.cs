using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Tetris
{
    internal class TetraminoO : Tetramino
    {
        public TetraminoO(Rectangle background)
        {
            Color = Color.Yellow;
            Type = 0;

            Initialize(Color, background);
        }

        public override void Rotate(Queue<Block>[] columns)
        {
            Initialize(Color, Blocks[0][0]);
        }

        protected override void SetProperties()
        {
            Columns = 2;
            Rows = 2;
        }
    }
}
