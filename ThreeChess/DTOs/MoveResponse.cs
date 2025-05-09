namespace ThreeChess.DTOs
{
    public class MoveResponse
    {
        public string StartCellId { get; set; }
        public string EndCellId { get; set; }
        public Guid GameId { get; set; }
    }
}
