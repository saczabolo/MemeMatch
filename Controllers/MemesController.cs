using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace MemeMatch.Controllers
{
    public class MemesController : Controller
    {
        public IActionResult Index()
        {
            
            var memesPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "memes"
            );

            
            if (!Directory.Exists(memesPath))
            {
                return Content("Folder wwwroot/memes NIE istnieje");
            }

            
            var memes = Directory.GetFiles(memesPath)
                .Where(f =>
                    f.EndsWith(".jpg") ||
                    f.EndsWith(".png") ||
                    f.EndsWith(".jpeg") ||
                    f.EndsWith(".webp"))
                .Select(f => "/memes/" + Path.GetFileName(f))
                .ToList();

            return View(memes);
        }
    }
}
