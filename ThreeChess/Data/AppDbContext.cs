using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ThreeChess.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasOne(u => u.PlayerProfileInfo)
                .WithOne(p => p.AppUser)
                .HasForeignKey<PlayerProfileInfo>(p => p.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PlayerProfileInfo>()
                .HasIndex(p => p.AppUserId)
                .IsUnique();
        }
    }
}
