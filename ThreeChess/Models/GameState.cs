using ThreeChess.Enums;
using ThreeChess.Models.CellElements;

namespace ThreeChess.Models
{
    public class GameState
    {
        public FigureColor CurrentTurnColor { get; set; }
        public GameStatus GameStatus { get; set; }
        public List<string> ActivePlayerIds { get; set; }
        public Dictionary<string, FigureInfo> FiguresLocation { get; set; }
    }
}
