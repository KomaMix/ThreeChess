using ThreeChess.Enums;
using ThreeChess.Models.CellElements;

namespace ThreeChess.Models
{
    public class GameState
    {
        public Guid Id { get; set; }
        public FigureColor CurrentTurnColor { get; set; }
        public GameStatus GameStatus { get; set; }
        public List<Guid> ActivePlayerIds { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Dictionary<string, FigureInfo> FiguresLocation { get; set; }
        public Dictionary<Guid, FigureColor> PlayerColors { get; set; }
        public Dictionary<Guid, TimeSpan> PlayerGameTimes { get; set; }
        public DateTime LastMoveTime { get; set; }
    }
}
