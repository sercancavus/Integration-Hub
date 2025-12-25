using IntegrationHub.Web.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json; // Bu satır çok önemli!

namespace IntegrationHub.Web.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ===========================
        // 1. BÖLÜM: AUTH (GİRİŞ/KAYIT)
        // ===========================
        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/Login", loginDto);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
                return result?.Token;
            }
            return null;
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Auth/register", registerDto);
            return response.IsSuccessStatusCode;
        }

        // ===========================
        // 2. BÖLÜM: ÜRÜNLER
        // ===========================
        public async Task<List<ProductViewModel>> GetProductsAsync(string token = null)
        {
            // Eğer token geldiyse Header'a ekle (Admin panelinden çağrılıyorsa)
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                // Token yoksa (Vitrin), Header'ı temizle ki eski token kalmasın
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }

            return await _httpClient.GetFromJsonAsync<List<ProductViewModel>>("api/Products");
        }

        public async Task<ProductViewModel> GetProductByIdAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<ProductViewModel>($"api/Products/{id}");
        }

        public async Task<bool> AddProductAsync(ProductViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(model.Name ?? ""), "Name");
                content.Add(new StringContent(model.Barcode ?? ""), "Barcode");
                content.Add(new StringContent(model.SKU ?? ""), "SKU");
                content.Add(new StringContent(model.SalePrice.ToString()), "SalePrice");
                content.Add(new StringContent(model.MarketPrice.ToString()), "MarketPrice");
                content.Add(new StringContent(model.StockQuantity.ToString()), "StockQuantity");
                content.Add(new StringContent(model.CategoryId.ToString()), "CategoryId");

                if (model.ImageUpload != null)
                {
                    var fileStream = model.ImageUpload.OpenReadStream();
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.ImageUpload.ContentType);
                    content.Add(fileContent, "imageFile", model.ImageUpload.FileName);
                }

                var response = await _httpClient.PostAsync("api/Products", content);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> UpdateProductAsync(ProductViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(model.Id.ToString()), "Id");
                content.Add(new StringContent(model.Name ?? ""), "Name");
                content.Add(new StringContent(model.Barcode ?? ""), "Barcode");
                content.Add(new StringContent(model.SKU ?? ""), "SKU");
                content.Add(new StringContent(model.SalePrice.ToString()), "SalePrice");
                content.Add(new StringContent(model.MarketPrice.ToString()), "MarketPrice");
                content.Add(new StringContent(model.StockQuantity.ToString()), "StockQuantity");
                content.Add(new StringContent(model.CategoryId.ToString()), "CategoryId");

                if (model.ImageUpload != null)
                {
                    var fileStream = model.ImageUpload.OpenReadStream();
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.ImageUpload.ContentType);
                    content.Add(fileContent, "imageFile", model.ImageUpload.FileName);
                }

                var response = await _httpClient.PutAsync($"api/Products/{model.Id}", content);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> DeleteProductAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"api/Products/{id}");
            return response.IsSuccessStatusCode;
        }

        // ===========================
        // 3. BÖLÜM: KATEGORİLER
        // ===========================
        public async Task<List<CategoryViewModel>> GetCategoriesAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<List<CategoryViewModel>>("api/Categories");
        }

        public async Task<CategoryViewModel> GetCategoryByIdAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<CategoryViewModel>($"api/Categories/{id}");
        }

        // IntegrationHub.Web -> Services -> ApiService.cs içinde

        public async Task<bool> AddCategoryAsync(CategoryViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // JSON olarak gönderiyoruz
            var response = await _httpClient.PostAsJsonAsync("api/Categories", model);

            // --- HATA YAKALAMA (CASUS KOD) ---
            if (!response.IsSuccessStatusCode)
            {
                // API'den gelen gerçek hata mesajını oku
                var errorContent = await response.Content.ReadAsStringAsync();

                // Hatayı ekranda görebilmek için fırlatıyoruz
                throw new Exception($"KATEGORİ KAYIT HATASI: {response.StatusCode} - {errorContent}");
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategoryAsync(CategoryViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PutAsJsonAsync($"api/Categories/{model.Id}", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCategoryAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"api/Categories/{id}");
            return response.IsSuccessStatusCode;
        }
    }

    // Yardımcı Class (Login cevabını karşılamak için)
    public class AuthResponseDto
    {
        public string Token { get; set; }
    }
}