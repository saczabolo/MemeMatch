using MemeMatch.Data;
using MemeMatch.Models;
using MemeMatch.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemeMatch.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly GameService _gameService;

        public GameController(AppDbContext context, GameService gameService)
        {
            _context = context;
            _gameService = gameService;
        }

        [HttpGet("start")]
        public IActionResult StartGame()
        {
            var memes = _gameService.GetRandomMemes();
            var prompt = _gameService.GetRandomPrompt();

            return Ok(new {prompt, memes});
        }

        [HttpPost("answer")]
        public IActionResult Answer(int userId, int memeId, int promptId)
        {
            bool isCorrect = memeId % 2 == 0;
            int score = isCorrect ? 10 : 0;

            var round = new GameRound
            {
                UserId = userId,
                SelectedMemeId = memeId,
                PromptId = promptId,
                IsCorrect = isCorrect,
                Score = score
            };

            _context.GameRounds.Add(round);
            _context.SaveChanges();

            return Ok(new {isCorrect, score});
        }
    }
}
