using Dawn_Winery.Data;
using Dawn_Winery.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dawn_Winery.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appDbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Menu()
        {
            return View();
        }
        public IActionResult Receipes()
        {
           
                return View();
            
        }
        public IActionResult Stocks()
        {
            return View();
        }


        public IActionResult Hammadde()
        {
            return View();
        }
        public IActionResult Uretim()
        {
            return View();
        }
        public IActionResult Mamul()
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