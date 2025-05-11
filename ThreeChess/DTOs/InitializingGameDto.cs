using ThreeChess.Enums;
using ThreeChess.Models;
using ThreeChess.Models.CellElements;

namespace ThreeChess.DTOs
{
    public class InitializingGameDto
    {
        public Guid GameId { get; set; }
        public string UserId { get; set; }
        public FigureColor ControlledColor { get; set; }
        public FigureColor CurrentTurnColor { get; set; }
        public List<CellItem> CellsLocation { get; set; }
        public List<string> ActivePlayerIds { get; set; }
        public Dictionary<string, FigureInfo> FiguresLocation { get; set; }
        public List<List<string>> Diagonals { get; set; }
        public List<List<string>> MainLines { get; set; }
        public List<List<string>> SecondaryLines { get; set; }
        public Dictionary<string, FigureColor> PlayerColors { get; set; }
        public Dictionary<string, TimeSpan> PlayerGameTimes { get; set; }
    }
}
