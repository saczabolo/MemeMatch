using MemeMatch.Data;
using MemeMatch.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MemeMatch.Controllers
{
    [Authorize]
    public class StatsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public StatsController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetStats()
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            if (currentUser == null) return RedirectToAction("Index", "Home");

            var rounds = _context.GameRounds
                .Where(g => g.User.Id == currentUser.Id)
                .ToList();

            int totalRounds = rounds.Count;
            int totalPoints = rounds.Sum(r => r.Score);
            double averagePoints = totalRounds > 0 ? (double)totalPoints / totalRounds : 0.0;

            ViewBag.TotalRounds = totalRounds;
            ViewBag.TotalPoints = totalPoints;
            ViewBag.AveragePoints = averagePoints;

            return View();
        }
    }
}
