namespace ThreeChess.Models
{
    public class Lobby
    {
        private static int _id = 1;
        public static int NewId()
        {
            return _id++;
        }

        public int Id { get; set; }
        public List<string> PlayerIds { get; set; } = new();
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
        public TimeSpan GameDuration { get; init; }
    }
}
