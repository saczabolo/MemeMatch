using Microsoft.EntityFrameworkCore;
using MemeMatch.Models;

namespace MemeMatch.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Meme> Memes => Set<Meme>();
        public DbSet<Prompt> Prompts => Set<Prompt>();
        public DbSet<GameRound> GameRounds => Set<GameRound>();
    }
}
