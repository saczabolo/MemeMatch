using MemeMatch.Data;
using MemeMatch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MemeMatch.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _web;

        public AdminController(AppDbContext context, IWebHostEnvironment web)
        {
            _context = context;
            _web = web;
        }

        public IActionResult Index()
        {
            var mp = _context.Prompts
                .Include(p => p.CorrectMeme)
                .ToList();
            return View(mp);
        }

        [HttpGet]
        public IActionResult AddMeme()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMeme(IFormFile formFile, string promptText)
        {
            if (formFile == null || formFile.Length == 0)
            {
                ViewBag.Error = "Nie wybrano pliku";
                return View();
            }

            if (string.IsNullOrWhiteSpace(promptText))
            {
                ViewBag.ErrorP = "Nie wpisano tesktu";
                return View();
            }

            var uploads = Path.Combine(_web.WebRootPath, "memes");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(formFile.FileName);
            var filePath = Path.Combine(uploads, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }

            var meme = new Meme
            {
                ImagePath = "/memes/" + fileName
            };

            _context.Memes.Add(meme);
            _context.SaveChanges();
            ViewBag.ImagePath = meme.ImagePath;

            var prompt = new Prompt
            {
                Text = promptText,
                CorrectMemeId = meme.Id
            };

            _context.Prompts.Add(prompt);
            _context.SaveChanges();

            ViewBag.Message = "Memik dodany szefie :>";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult EditMeme(int id)
        {
            var prompt = _context.Prompts
                .Include(p => p.CorrectMeme)
                .FirstOrDefault(p => p.Id == id);

            if (prompt == null) return NotFound();

            return View(prompt);
        }

        [HttpPost]
        public IActionResult EditMeme(int promptId, string promptText)
        {
            var prompt = _context.Prompts.Find(promptId);

            if (prompt == null) return NotFound();

            prompt.Text = promptText;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult DeleteMeme(int id)
        {
            var prompt = _context.Prompts
                .Include(p => p.CorrectMeme)
                .FirstOrDefault(p => p.Id == id);

            if (prompt == null) return NotFound();
            return View(prompt);
        } 

        [HttpPost]
        public IActionResult DeleteMeme2(int id)
        {
            var prompt = _context.Prompts
                .Include(p => p.CorrectMeme)
                .FirstOrDefault(p => p.Id == id);

            if (prompt == null) return NotFound();

            var meme = prompt.CorrectMeme;

            if (meme != null && !string.IsNullOrWhiteSpace(meme.ImagePath))
            {
                var relativePath = meme.ImagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                var filePath = Path.Combine(_web.WebRootPath, relativePath);

                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);

            }
           
            _context.Prompts.Remove(prompt);
            if (meme != null)
                _context.Memes.Remove(meme);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
