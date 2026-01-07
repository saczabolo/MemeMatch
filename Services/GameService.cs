using MemeMatch.Data;
using MemeMatch.Models;


namespace MemeMatch.Services
{
    public class GameService
    {
        private readonly AppDbContext _context;

        public GameService(AppDbContext context)
        {
            _context = context;
        }

        public List<Meme> GetRandomMemes()
        {
            return _context.Memes
            .Where(m =>m.IsActive)
            .OrderBy(x => Guid.NewGuid())
            .Take(5)
            .ToList();
        }

        public Prompt GetRandomPrompt()
        {
            return _context.Prompts
                .OrderBy(x => Guid.NewGuid())
                .First();
        }
    }
}
