using Microsoft.AspNetCore.Identity;

namespace ThreeChess.Data
{
    public class AppUser : IdentityUser
    {
        public double Score { get; set; } = 1000;
        public PlayerProfileInfo PlayerProfileInfo { get; set; }
    }
}
