namespace ThreeChess.DTOs
{
    public class MoveRequest
    {
        public string UserId { get; set; }
        public string StartCellId { get; set; }
        public string EndCellId { get; set; }
        public Guid GameId { get; set; }
    }
}
