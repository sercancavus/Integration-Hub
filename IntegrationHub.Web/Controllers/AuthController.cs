using IntegrationHub.Web.Models;
using IntegrationHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationHub.Web.Controllers
{
    public class AuthController : Controller
    {
        // Controller sadece Servisi tanır, HttpClient'ı tanımaz.
        private readonly IApiService _apiService;

        public AuthController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // --- REGISTER (KAYIT OL) ---
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Burası servisi çağırır. Servis hata fırlatırsa uygulama patlar ve hatayı görürüz.
                var result = await _apiService.RegisterAsync(model);

                if (result)
                {
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("", "Kayıt başarısız.");
            }
            return View(model);
        }

        // --- LOGIN (GİRİŞ YAP) ---
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await _apiService.LoginAsync(model);
                if (!string.IsNullOrEmpty(token))
                {
                    HttpContext.Session.SetString("JWToken", token);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}