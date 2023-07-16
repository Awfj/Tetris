using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    internal class TetraminoO : Tetramino
    {
        public TetraminoO(GraphicsDevice graphicsDevice, Rectangle background)
        {
            Color = Color.Yellow;
            Type = 0;

            Initialize(graphicsDevice, Color, background);
        }

        public override void Rotate(GraphicsDevice graphicsDevice)
        {
            Initialize(graphicsDevice, Color, Blocks[0][0]);
        }

        protected override void SetProperties()
        {
            Columns = 2;
            Rows = 2;
        }
    }
}
