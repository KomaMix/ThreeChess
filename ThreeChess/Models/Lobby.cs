namespace ThreeChess.Models
{
    public class Lobby
    {
        public Guid Id { get; set; }
        public List<Guid> PlayerIds { get; set; } = new();
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
        public TimeSpan GameDuration { get; init; }
    }
}
