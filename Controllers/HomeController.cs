using Microsoft.AspNetCore.Mvc;

namespace MemeMatch.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}