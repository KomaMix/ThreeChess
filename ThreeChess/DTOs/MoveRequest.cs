namespace ThreeChess.DTOs
{
    public class MoveRequest
    {
        public Guid UserId { get; set; }
        public string StartCellId { get; set; }
        public string EndCellId { get; set; }
        public Guid GameId { get; set; }
    }
}
