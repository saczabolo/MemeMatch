using MemeMatch.Data;
using MemeMatch.Models;
using MemeMatch.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemeMatch.Controllers
{
    public class GameController : Controller
    {
        private readonly AppDbContext _context;
        private readonly GameService _gameService;

        public GameController(AppDbContext context, GameService gameService)
        {
            _context = context;
            _gameService = gameService;
        }


        [HttpGet]
        public IActionResult StartGame()
        {
            try
            {
                var (prompt, memes) = _gameService.StartGame();

                ViewBag.Prompt = prompt;
                ViewBag.Memes = memes;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult Answer(int userId, int memeId, int promptId)
        {
            var prompt = _context.Prompts.Find(promptId);

            if (prompt == null)
            {
                return RedirectToAction(nameof(StartGame));
            }
            
            bool isCorrect = memeId == prompt.CorrectMemeId;
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

            TempData["IsCorrect"] = isCorrect;
            TempData["Score"] = score;

            return RedirectToAction(nameof(Result));
        }

        public IActionResult Result()
        {
            if (TempData["IsCorrect"] == null)
            {
                return RedirectToAction(nameof(StartGame));
            }

            ViewBag.IsCorrect = (bool)TempData["IsCorrect"];
            ViewBag.Score = (int)TempData["Score"];

            return View();
        }
    }
}
