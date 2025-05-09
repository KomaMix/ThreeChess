namespace ThreeChess.Enums
{
    public enum FigureColor
    {
        White,
        Black,
        Red
    }

    public static class FigureColorExtensions
    {
        public static FigureColor NextColor(this FigureColor color)
        {
            return (FigureColor)(((int)color + 1) % 3);
        }
    }

}
