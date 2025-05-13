using System.ComponentModel.DataAnnotations.Schema;

namespace ThreeChess.Data
{
    public class PlayerProfileInfo
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int NumberOfCompletedGames { get; set; }
        public int NumberOfWins { get; set; }
        public int NumberOfLosses { get; set; }
        public int NumberOfDraws { get; set; }
    }
}
