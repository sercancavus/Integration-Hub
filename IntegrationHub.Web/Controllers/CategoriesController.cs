using IntegrationHub.Web.Models;
using IntegrationHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationHub.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IApiService _apiService;

        public CategoriesController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var categories = await _apiService.GetCategoriesAsync(token);
            return View(categories); // Listeyi View'a gönder
        }
        [HttpGet]
        public IActionResult Create()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            return View();
        }

        // --- YENİ KATEGORİ KAYDETME İŞLEMİ (POST) ---
        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                var result = await _apiService.AddCategoryAsync(model, token);
                if (result)
                {
                    return RedirectToAction("Index"); // Başarılıysa listeye dön
                }
                ModelState.AddModelError("", "Kategori eklenirken bir hata oluştu.");
            }

            return View(model);
        }
        // --- DÜZENLEME SAYFASI (GET) ---
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var category = await _apiService.GetCategoryByIdAsync(id, token);
            if (category == null) return NotFound();

            return View(category);
        }

        // --- DÜZENLEME İŞLEMİ (POST) ---
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                var result = await _apiService.UpdateCategoryAsync(model, token);
                if (result)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Güncelleme başarısız.");
            }
            return View(model);
        }

        // --- SİLME ONAY SAYFASI (GET) ---
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var category = await _apiService.GetCategoryByIdAsync(id, token);
            if (category == null) return NotFound();

            return View(category);
        }

        // --- SİLME İŞLEMİ (POST) ---
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var result = await _apiService.DeleteCategoryAsync(id, token);
            if (result)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Silme işlemi başarısız.");
            return View(); // Hata olursa sayfada kal
        }
    }
}