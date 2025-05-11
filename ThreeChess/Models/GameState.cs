using ThreeChess.Enums;
using ThreeChess.Models.CellElements;

namespace ThreeChess.Models
{
    public class GameState
    {
        public Guid Id { get; set; }
        public FigureColor CurrentTurnColor { get; set; }
        public GameStatus GameStatus { get; set; }
        public List<string> ActivePlayerIds { get; set; }
        public DateTime CreatedAt { get; set; }
        public Dictionary<string, FigureInfo> FiguresLocation { get; set; }
        public Dictionary<string, FigureColor> PlayerColors { get; set; }
        public Dictionary<string, TimeSpan> PlayerGameTimes { get; set; }
        public DateTime LastMoveTime { get; set; }
    }
}
