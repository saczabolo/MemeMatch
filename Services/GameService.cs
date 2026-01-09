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
                .AsEnumerable()
                .OrderBy(_ => Guid.NewGuid())
                .FirstOrDefault();

            if (prompt == null)
                throw new Exception("Brak promptów w bazie.");

            var allMemes = _context.Memes
                .Where(m => m.IsActive)
                .ToList();

            if (!allMemes.Any())
                throw new Exception("Brak memów w bazie.");

            var selectedMemes = new List<Meme>();
            var correctMeme = allMemes.First(m => m.Id == prompt.CorrectMemeId);
            selectedMemes.Add(correctMeme);

            var otherMemes = allMemes
                .Where(m => m.Id != prompt.CorrectMemeId)
                .OrderBy(_ => Guid.NewGuid())
                .Take(4)  
                .ToList();

            selectedMemes.AddRange(otherMemes);

            selectedMemes = selectedMemes.OrderBy(_ => Guid.NewGuid()).ToList();

            return (prompt, selectedMemes);
        }
    }
}
