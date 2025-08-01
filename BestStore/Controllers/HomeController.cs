using BestStore.Data;
using BestStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BestStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;
        private const int PageSize = 10; // số sản phẩm mỗi trang

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this.context = context;
        }
        [Authorize]
        public IActionResult Index(string? searchString, int page = 1)
        {
            var products = context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }

            int totalItems = products.Count();
            var productList = products
                .OrderBy(p => p.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewData["CurrentFilter"] = searchString;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / PageSize);

            return View(productList);
        }


        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult RecentlyUpdated()
        {
            return View();
        }
        public IActionResult ListGame()
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
