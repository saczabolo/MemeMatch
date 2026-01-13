using MemeMatch.Data;
using MemeMatch.Models;
using Microsoft.AspNetCore.Mvc;


namespace MemeMatch.Controllers
{
    public class RankingController : Controller
    {
        private readonly AppDbContext _context;

        public RankingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetRanking()
        {
            var ranking = _context.Users
                .Select(u => new RankingView
                {
                    Username = u.Username,
                    Points = u.GameRounds.Sum(g => g.Score)
                })
                .OrderByDescending(x => x.Points)
                .ToList();

            return View(ranking);
        }
    }
}
