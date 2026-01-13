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

        public (Prompt prompt, List<Meme> memes) StartGame()
        {
            var prompt = _context.Prompts
                .OrderBy(x => Guid.NewGuid())
                .First();
            var correctMemeId = prompt.CorrectMemeId;

            var memes = _context.Memes
                .OrderBy(x => Guid.NewGuid())
                .Take(4)
                .ToList();

            if (prompt == null)
                throw new Exception("Brak promptów w bazie");

            if (memes == null)
                throw new Exception("Brak memów w bazie");

            if (!memes.Any(m => m.Id == correctMemeId))
            {
                memes[0] = _context.Memes.Find(correctMemeId);
            }

            return (prompt, memes);
        }
    }
}
