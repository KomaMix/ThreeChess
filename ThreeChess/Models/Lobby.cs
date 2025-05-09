namespace ThreeChess.Models
{
    public class Lobby
    {
        private static int _id = 1;

        public int Id { get; } = _id++;
        public List<string> PlayerIds { get; } = new();
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
        public TimeSpan GameDuration { get; init; }
    }
}
