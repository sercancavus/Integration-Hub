using Microsoft.AspNetCore.Mvc;
using IntegrationHub.Web.Services;
using IntegrationHub.Web.Models;

namespace IntegrationHub.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _apiService; // <-- Servisi burada tanımlıyoruz

        // Constructor (Yapıcı Metot)
        public AuthController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var token = await _apiService.LoginAsync(username, password);
            if (!string.IsNullOrEmpty(token))
            {
                // Token'ı kaydet (Örn: Session veya Cookie)
                HttpContext.Session.SetString("JWToken", token);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Giriş başarısız";
            return View();
        }

        public IActionResult Logout()
        {
            // Oturumu temizle (Token'ı sil)
            HttpContext.Session.Clear();

            // Giriş sayfasına yönlendir
            return RedirectToAction("Login");
        }
    }
}