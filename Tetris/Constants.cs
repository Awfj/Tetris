namespace Tetris
{
    internal sealed class Constants
    {
        public const int Delay = 10;

        public static int TotalColumns = Background.Width / Block.Length;
        public static int TotalRows = Background.Height / Block.Length;
    }
}
