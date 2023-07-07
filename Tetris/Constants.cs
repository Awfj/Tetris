namespace Tetris
{
    internal sealed class Constants
    {
        public const int BlockDimension = 20;

        public const int BackgroundWidth = 300;
        public const int BackgroundHeight = 400;

        public const int Delay = 15;

        public static int TotalColumns = Constants.BackgroundWidth / Constants.BlockDimension;
        public static int TotalRows = Constants.BackgroundHeight / Constants.BlockDimension;
    }
}
