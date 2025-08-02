using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThreeChess.Models;

namespace ThreeChess.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<OldGameInfo> OldGameInfos { get; set; }
        public DbSet<UserOldGameInfo> UserOldGameInfos { get; set; }

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

            builder.Entity<UserOldGameInfo>()
                .HasKey(ug => new { ug.AppUserId, ug.OldGameInfoId });

            builder.Entity<UserOldGameInfo>()
                .HasOne(ug => ug.AppUser)
                .WithMany()
                .HasForeignKey(ug => ug.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserOldGameInfo>()
                .HasOne(ug => ug.OldGameInfo)
                .WithMany()
                .HasForeignKey(ug => ug.OldGameInfoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
