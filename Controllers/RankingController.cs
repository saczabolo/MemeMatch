using MemeMatch.Data;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace MemeMatch.Controllers
{
    [ApiController]
    [Route("api/ranking")]
    public class RankingController : ControllerBase
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
                .Select(u => new
                {
                    u.Username,
                    Points = u.GameRounds.Sum(g => g.Score)
                })
                .OrderByDescending(x => x.Points)
                .ToList();

            return Ok(ranking);
        }
    }
}
