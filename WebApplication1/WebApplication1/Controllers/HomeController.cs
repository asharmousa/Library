using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext _dbContext;
        public HomeController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        //public IActionResult Search()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Search(string searchTerm)
        //{
        //    var query = _dbContext.Books.AsQueryable();

        //    if (!string.IsNullOrWhiteSpace(searchTerm))
        //    {
        //        query = query.Where(b => b.BookTitle.Contains(searchTerm));
        //    }
        //    var results = await query.OrderBy(b => b.BookId).ToListAsync();
        //    ViewData["searchTerm"] = searchTerm;
        //    return RedirectToAction("showBookForStud","Book", results);
        //}
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult about()
        {
            return View();
        }
        public IActionResult contact()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
