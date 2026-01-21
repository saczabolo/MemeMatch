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
        public async Task<IActionResult> GetRanking(string sortOrder = "desc")
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var adminIds = admins.Select(a => a.Id);

            var ranking = await _context.Users
                .Where(u => !adminIds.Contains(u.Id))
                .Select(u => new
                {
                    Username = u.UserName,
                    Points = u.GameRounds.Sum(g => (int?)g.Score) ?? 0
                })
                .ToListAsync();

            ranking = sortOrder.ToLower() == "asc"
                ? ranking.OrderBy(x => x.Points).ToList()
                : ranking.OrderByDescending(x => x.Points).ToList();
                
            return View(ranking);
        }
    }
}