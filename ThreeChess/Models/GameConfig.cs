using ThreeChess.Enums;

namespace ThreeChess.Models
{
    public class GameConfig
    {
        public FigureColor Color { get; set; }
        public List<CellItem> CellsLocation { get; set; }
        public Dictionary<string, FigureInfo> FiguresLocation { get; set; }
        public List<List<string>> Diagonals { get; set; }
        public List<List<string>> MainLines { get; set; }
        public List<List<string>> SecondaryLines { get; set; }
    }
}
