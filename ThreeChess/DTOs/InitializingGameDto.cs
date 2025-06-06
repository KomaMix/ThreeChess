using ThreeChess.Enums;
using ThreeChess.Models;
using ThreeChess.Models.CellElements;

namespace ThreeChess.DTOs
{
    public class InitializingGameDto
    {
        public Guid GameId { get; set; }
        public Guid UserId { get; set; }
        public GameStatus GameStatus { get; set; }
        public FigureColor ControlledColor { get; set; }
        public FigureColor CurrentTurnColor { get; set; }
        public List<CellItem> CellsLocation { get; set; }
        public List<Guid> ActivePlayerIds { get; set; }
        public Dictionary<string, FigureInfo> FiguresLocation { get; set; }
        public List<List<string>> Diagonals { get; set; }
        public List<List<string>> MainLines { get; set; }
        public List<List<string>> SecondaryLines { get; set; }
        public Dictionary<Guid, FigureColor> PlayerColors { get; set; }
        public Dictionary<Guid, TimeSpan> PlayerGameTimes { get; set; }
        public List<Move> MoveHistory { get; set; }
    }
}
