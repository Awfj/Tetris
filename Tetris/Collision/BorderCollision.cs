namespace Tetris
{
    internal static class BorderCollision
    {
        public static bool CheckIfCollidesWithSideBorder(ITetramino element, Direction direction)
        {
            return direction switch
            {
                Direction.Left => CheckIfCollidesWithSideBorderLeft(element),
                Direction.Right => CheckIfCollidesWithSideBorderRight(element),
                Direction.Down => CheckIfCollidesWithSideBorderDown(element),
                _ => false,
            };
        }

        private static bool CheckIfCollidesWithSideBorderLeft(ITetramino element)
        {
            foreach (var block in element.Blocks[0])
            {
                if (block.Column <= 0) return true;
            }
            return false;
        }

        private static bool CheckIfCollidesWithSideBorderRight(ITetramino element)
        {
            foreach (var block in element.Blocks[^1])
            {
                if (block.Column >= Constants.TotalColumns - 1) return true;
            }
            return false;
        }

        private static bool CheckIfCollidesWithSideBorderDown(ITetramino element)
        {
            foreach (var column in element.Blocks)
            {
                if (column[^1].Row >= 19) return true;
            }
            return false;
        }
    }
}
