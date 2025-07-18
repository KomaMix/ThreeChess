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
                .HasKey(ug => new { ug.AppUserId, ug.OldGameInfoId }); // Составной ключ

            builder.Entity<UserOldGameInfo>()
                .HasOne(ug => ug.AppUser)
                .WithMany() // Если в AppUser нет коллекции UserOldGameInfo, оставьте пустым
                .HasForeignKey(ug => ug.AppUserId)
                .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление связей при удалении пользователя

            builder.Entity<UserOldGameInfo>()
                .HasOne(ug => ug.OldGameInfo)
                .WithMany() // Если в OldGameInfo нет коллекции UserOldGameInfo, оставьте пустым
                .HasForeignKey(ug => ug.OldGameInfoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
