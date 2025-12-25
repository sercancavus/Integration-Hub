using IntegrationHub.Web.Models;
using IntegrationHub.Web.Services; // IApiService burada
using Microsoft.AspNetCore.Mvc;

namespace IntegrationHub.Web.Controllers
{
    public class ProductsController : Controller
    {
        // HATA BURADAYDI: Eskiden 'ApiService' yazıyordu, 'IApiService' yaptık.
        private readonly IApiService _apiService;

        // Constructor'da da Interface istiyoruz
        public ProductsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            // Token'ı Session'dan al
            var token = HttpContext.Session.GetString("JWToken");

            // Eğer token yoksa (Oturum düşmüşse) Giriş'e gönder
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Servisten ürünleri çek
            var products = await _apiService.GetProductsAsync(token);
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            // Kategorileri doldurmak için servisi çağırıyoruz
            var categories = await _apiService.GetCategoriesAsync(token);
            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (ModelState.IsValid)
            {
                // Ürün ekleme servisini çağır
                var result = await _apiService.AddProductAsync(model, token);

                if (result)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Ürün eklenirken bir hata oluştu.");
            }

            // Hata varsa kategorileri tekrar yükle ki Dropdown boş kalmasın
            var categories = await _apiService.GetCategoriesAsync(token);
            ViewBag.Categories = categories;

            return View(model);
        }

        // --- DÜZENLEME SAYFASINI AÇ (GET) ---
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            // 1. Düzenlenecek ürünü API'den çek
            var product = await _apiService.GetProductByIdAsync(id, token);

            // 2. Kategorileri de çek (Dropdown için lazım)
            var categories = await _apiService.GetCategoriesAsync(token);
            ViewBag.Categories = categories;

            if (product == null) return NotFound();

            return View(product);
        }

        // --- DEĞİŞİKLİKLERİ KAYDET (POST) ---
        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                // 3. Güncellemeyi Servise Gönder
                var result = await _apiService.UpdateProductAsync(model, token);

                if (result)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Güncelleme başarısız oldu.");
            }

            // Hata varsa sayfayı tekrar yükle (Kategorileri unutma!)
            var categories = await _apiService.GetCategoriesAsync(token);
            ViewBag.Categories = categories;

            return View(model);
        }
        // --- SİLME ONAY SAYFASI (GET) ---
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            // Silinecek ürünü ekrana getirelim ki kullanıcı neyi sildiğini görsün
            var product = await _apiService.GetProductByIdAsync(id, token);

            if (product == null) return NotFound();

            return View(product);
        }

        // --- SİLME İŞLEMİ (POST) ---
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            // API'ye "Bunu sil" emrini veriyoruz
            var result = await _apiService.DeleteProductAsync(id, token);

            if (result)
            {
                return RedirectToAction("Index");
            }

            // Silinemezse hata mesajı göster
            ModelState.AddModelError("", "Silme işlemi başarısız.");
            return View();
        }
    }
}