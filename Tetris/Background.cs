using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris;

internal class Background
{
    public const int Width = 300;
    public const int Height = 400;

    public Rectangle rectangle;
    public Texture2D texture;

    public Background(GraphicsDevice graphicsDevice)
    {
        rectangle = new(
            graphicsDevice.Viewport.Width / 2 - Width / 2,
            graphicsDevice.Viewport.Height / 2 - Height / 2,
            Width, Height);

        texture = new(graphicsDevice, 1, 1);
        texture.SetData(new Color[] { Color.White });
    }

    public void Draw()
    {
        Globals.SpriteBatch.Draw(texture, rectangle, Color.Pink);
    }
}
