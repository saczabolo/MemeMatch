using MemeMatch.Data;
using MemeMatch.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MemeMatch.Controllers
{
    public class RankingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public RankingController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetRanking(string sortOrder = "desc")
        {
            var users = _context.Users
                .Include(u => u.GameRounds)
                .Where(u => u.GameRounds.Any())
                .ToList();

            var ranking = users
                .Where(u => !_userManager.IsInRoleAsync(u, "Admin").Result)
                .Select(u => new
                {
                    Username = u.UserName,
                    Points = u.GameRounds.Sum(g => g.Score)
                });

            ranking = sortOrder.ToLower() == "asc"
                ? ranking.OrderBy(x => x.Points)
                : ranking.OrderByDescending(x => x.Points);
                

            return View(ranking.ToList());
        }
    }
}
