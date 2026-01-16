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

        private const int MaxRounds = 5;

        [HttpGet]
        public IActionResult StartGame()
        {
            var gameInProgress = HttpContext.Session.GetString("GameInProgress");

            if (gameInProgress != "true") return RedirectToAction(nameof(StartNewGame));
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
        public IActionResult StartNewGame()
        {
            HttpContext.Session.SetInt32("Round", 1);
            HttpContext.Session.SetInt32("TotalScore", 0);
            HttpContext.Session.SetString("GameInProgress", "true");

            return RedirectToAction(nameof(NextRound));
        }

        [HttpGet]
        public IActionResult NextRound()
        {
            var gameInProgress = HttpContext.Session.GetString("GameInProgress");

            if (gameInProgress != "true") return RedirectToAction(nameof(StartNewGame));

            int round = HttpContext.Session.GetInt32("Round") ?? 1;
            int totalScore = HttpContext.Session.GetInt32("TotalScore") ?? 0;

            if (round > MaxRounds)
            {
                HttpContext.Session.Remove("GameInProgress");

                if (totalScore >= 30) return RedirectToAction(nameof(WinWin));
                return RedirectToAction(nameof(GameOver));
            }  

            var (prompt, memes) = _gameService.StartGame();
            
            ViewBag.Prompt = prompt;
            ViewBag.Memes = memes;
            ViewBag.Round = round;
            ViewBag.MaxRounds = MaxRounds;

            return View("StartGame");
        }

        [HttpPost]
        public async Task<IActionResult> Answer(int memeId, int promptId)
        {
            var user = await _userManager.GetUserAsync(User);
            
            var prompt = _context.Prompts.Find(promptId);

            if (prompt == null)
            {
                return RedirectToAction(nameof(NextRound));
            }
            
            bool isCorrect = memeId == prompt.CorrectMemeId;
            int score = isCorrect ? 10 : 0;

            int totalScore = HttpContext.Session.GetInt32("TotalScore") ?? 0;
            HttpContext.Session.SetInt32("TotalScore", totalScore + score);

            int round = HttpContext.Session.GetInt32("Round") ?? 1;
            HttpContext.Session.SetInt32("Round", round + 1);

            _context.GameRounds.Add(new GameRound
            {
                UserId = user.Id,
                SelectedMemeId = memeId,
                PromptId = promptId,
                IsCorrect = isCorrect,
                Score = score
            });

            _context.SaveChanges();

            TempData["IsCorrect"] = isCorrect;
            TempData["Score"] = score;

            return RedirectToAction(nameof(Result));
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Result()
        {
            if (HttpContext.Session.GetString("GameInProgress") != "true")
                return RedirectToAction("Index", "Home");
            
            ViewBag.IsCorrect = TempData["IsCorrect"];
            ViewBag.Score = TempData["Score"];

            if (TempData["IsCorrect"] == null) return RedirectToAction(nameof(NextRound));

            return View();
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult GameOver()
        {
            ViewBag.TotalScore = HttpContext.Session.GetInt32("TotalScore") ?? 0;
            ViewBag.MaxRounds = MaxRounds;

            return View();
        }

        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult WinWin()
        {
            int totalScore = HttpContext.Session.GetInt32("TotalScore") ?? 0;
            int roundsPlayed = (HttpContext.Session.GetInt32("Round") ?? 1) - 1;

            ViewBag.TotalScore = totalScore;
            ViewBag.RoundsPlayed = roundsPlayed;

            return View();
        }
    }
}
