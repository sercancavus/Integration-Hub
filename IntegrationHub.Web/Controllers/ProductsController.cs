using IntegrationHub.Web.Models;
using IntegrationHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationHub.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApiService _apiService;

        public ProductsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Session'dan Token'ı al
            var token = HttpContext.Session.GetString("JWToken");

            // 2. Token yoksa Login'e at (Güvenlik)
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            // 3. Servise git ve ürünleri getir
            var products = await _apiService.GetProductsAsync(token);

            // 4. Veriyi View'a gönder
            return View(products);
        }

        // 1. Sayfayı (Formu) Gösteren Metot
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("JWToken");

            // Eğer Token yoksa Login'e at (Güvenlik)
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Kategorileri API'den çek
            var categories = await _apiService.GetCategoriesAsync(token);

            // ÖNEMLİ DOKUNUŞ: Eğer API'den null gelirse veya hata olursa,
            // "categories" değişkeni null kalabilir. Bunu boş bir listeye çevirelim ki sayfa patlamasın.
            ViewBag.Categories = categories ?? new List<CategoryViewModel>();

            return View();
        }

        // 2. Formdan Gelen Veriyi Kaydeden Metot
        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            var token = HttpContext.Session.GetString("JWToken");
            var categories = await _apiService.GetCategoriesAsync(token);
            ViewBag.Categories = categories;

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            // Servise gönder
            bool result = await _apiService.AddProductAsync(model, token);

            if (result)
            {
                return RedirectToAction("Index"); // Başarılıysa listeye dön
            }

            ViewBag.Error = "Ürün eklenirken bir hata oluştu.";
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            await _apiService.DeleteProductAsync(id, token);

            // Sildikten sonra listeyi yenilemek için Index'e geri dön
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            // Hem ürünü hem kategorileri getir
            var product = await _apiService.GetProductByIdAsync(id, token);
            var categories = await _apiService.GetCategoriesAsync(token);

            if (product == null) return RedirectToAction("Index");

            // Kategorileri çantaya koy
            ViewBag.Categories = categories;

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            var token = HttpContext.Session.GetString("JWToken");

            // API'ye güncel veriyi gönderiyoruz
            var result = await _apiService.UpdateProductAsync(model, token);

            if (result)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Error = "Güncelleme başarısız.";
            return View(model);
        }
    }
}