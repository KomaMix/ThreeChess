namespace ThreeChess.Models
{
    public class Lobby
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<string> PlayerIds { get; set; } = new();
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
    }
}
