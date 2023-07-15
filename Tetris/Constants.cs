namespace Tetris
{
    internal sealed class Constants
    {
        public const int Delay = 15;

        public static int TotalColumns = Background.Width / Block.Length;
        public static int TotalRows = Background.Height / Block.Length;
    }
}
