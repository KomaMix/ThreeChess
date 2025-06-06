namespace ThreeChess.DTOs
{
    public class MoveResponse
    {
        public Guid GameId { get; set; }
        public Guid UserId { get; set; }
        public string StartCellId { get; set; }
        public string EndCellId { get; set; }
        public Dictionary<Guid, TimeSpan> PlayerGameTimes { get; set; }
    }
}
