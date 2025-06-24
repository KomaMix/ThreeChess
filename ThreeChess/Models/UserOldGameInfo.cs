using ThreeChess.Data;

namespace ThreeChess.Models
{
    public class UserOldGameInfo
    {
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid OldGameInfoId { get; set; }
        public OldGameInfo OldGameInfo { get; set; }
    }
}
