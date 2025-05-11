namespace ThreeChess.DTOs
{
    public class MoveResponse
    {
        public Guid GameId { get; set; }
        public string UserId { get; set; }
        public string StartCellId { get; set; }
        public string EndCellId { get; set; }
        public Dictionary<string, TimeSpan> PlayerGameTimes { get; set; }
    }
}
