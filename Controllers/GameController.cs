using MemeMatch.Data;
using MemeMatch.Models;
using MemeMatch.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MemeMatch.Controllers
{
    
    public class GameController : Controller
    {
        private readonly AppDbContext _context;
        private readonly GameService _gameService;
        private readonly UserManager<User> _userManager;

        public GameController(AppDbContext context, GameService gameService, UserManager<User> userManager)
        {
            _context = context;
            _gameService = gameService;
            _userManager = userManager;
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
        public async Task<IActionResult> Answer(int memeId, int promptId)
        {
            var user = await _userManager.GetUserAsync(User);
            

            var prompt = _context.Prompts.Find(promptId);

            if (prompt == null)
            {
                return RedirectToAction(nameof(StartGame));
            }
            
            bool isCorrect = memeId == prompt.CorrectMemeId;
            int score = isCorrect ? 10 : 0;

            var round = new GameRound
            {
                UserId = user.Id,
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
