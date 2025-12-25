using IntegrationHub.Web.Models;
using IntegrationHub.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationHub.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiService _apiService;

        public AuthController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // --- GİRİŞ YAP (LOGIN) ---
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // ÇÖZÜM BURADA: LoginModel'i LoginDto'ya çeviriyoruz
            var loginDto = new LoginDto
            {
                Email = model.Email,
                Password = model.Password
            };

            // Artık servise DTO gönderiyoruz
            var token = await _apiService.LoginAsync(loginDto);

            if (!string.IsNullOrEmpty(token))
            {
                HttpContext.Session.SetString("JWToken", token);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Giriş başarısız. Bilgileri kontrol edin.");
            return View(model);
        }

        // --- KAYIT OL (REGISTER) ---
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // ÇÖZÜM BURADA: RegisterModel'i RegisterDto'ya çeviriyoruz
            var registerDto = new RegisterDto
            {
                FullName = model.FullName, // Eğer modelde bu alan yoksa, burayı silebilirsin veya modeline ekleyebilirsin
                Email = model.Email,
                Password = model.Password
                // Username'i DTO içinde otomatik Email'den alıyor, o yüzden göndermeye gerek yok
            };

            // Servise DTO gönderiyoruz
            var result = await _apiService.RegisterAsync(registerDto);

            if (result)
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Kayıt olurken bir hata oluştu.");
            return View(model);
        }

        // --- ÇIKIŞ YAP (LOGOUT) ---
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}