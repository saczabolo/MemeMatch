using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MemeMatch.Models;
using Microsoft.AspNetCore.Identity;

namespace MemeMatch.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        
        public DbSet<Meme> Memes => Set<Meme>();
        public DbSet<Prompt> Prompts => Set<Prompt>();
        public DbSet<GameRound> GameRounds => Set<GameRound>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "ADMIN_ROLE_ID", Name = "Admin", NormalizedName = "ADMIN"},
                new IdentityRole { Id = "USER_ROLE_ID", Name = "User", NormalizedName = "USER" }
            );

            modelBuilder.Entity<GameRound>()
                .HasOne(gr => gr.User)
                .WithMany(u => u.GameRounds)
                .HasForeignKey(gr => gr.UserId);

            var hasher = new PasswordHasher<User>();
            var admin = new User
            {
                Id = "ADMIN_ID",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@wp.pl",
                NormalizedEmail = "ADMIN@WP.PL",
                EmailConfirmed = true,
            };

            admin.PasswordHash = hasher.HashPassword(admin, "admin");

            modelBuilder.Entity<User>().HasData(admin);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "ADMIN_ID",
                    RoleId = "ADMIN_ROLE_ID",
                });
        }
    }
}
