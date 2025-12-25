using IntegrationHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationHub.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiService _apiService;

        public HomeController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            // Token vermeden çaðýrýyoruz (null gidecek)
            var products = await _apiService.GetProductsAsync();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}