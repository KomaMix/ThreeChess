namespace ThreeChess.DTOs
{
    public class LobbyDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PlayerDto> Players { get; set; } = new();
    }
}
