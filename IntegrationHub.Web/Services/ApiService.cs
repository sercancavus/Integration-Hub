using IntegrationHub.Web.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace IntegrationHub.Web.Services
{
    public class ApiService : IApiService
    {
        // --- İŞTE EKSİK OLAN PARÇA BURASIYDI ---
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        // ----------------------------------------

        // 1. KAYIT OL (HATA YAKALAYICI MOD AKTİF)
        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            // API'ye isteği atıyoruz
            var response = await _httpClient.PostAsync("api/Auth/Register", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var hata = await response.Content.ReadAsStringAsync();
                throw new Exception($"API HATASI: {hata}"); // Hatayı fırlatan yer burası
            }
            return response.IsSuccessStatusCode;
        }

        // 2. GİRİŞ YAP
        public async Task<string> LoginAsync(LoginModel model)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Auth/Login", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                // dynamic kullanarak gelen JSON'ı esnek bir yapıya çeviriyoruz
                dynamic tokenObj = JsonConvert.DeserializeObject(responseData);
                return tokenObj.token;
            }
            return null;
        }

        // 3. KATEGORİLERİ GETİR
        public async Task<List<CategoryViewModel>> GetCategoriesAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return new List<CategoryViewModel>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("api/Categories");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CategoryViewModel>>(jsonData);
            }
            return new List<CategoryViewModel>();
        }

        // 4. ÜRÜNLERİ GETİR
        public async Task<List<ProductViewModel>> GetProductsAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return new List<ProductViewModel>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("api/Products");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ProductViewModel>>(jsonData);
            }
            return new List<ProductViewModel>();
        }

        // 5. ÜRÜN EKLE (RESİMLİ)
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

        // 6. ÜRÜN DETAY
        public async Task<ProductViewModel> GetProductByIdAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"api/Products/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductViewModel>(jsonData);
            }
            return null;
        }

        // 7. ÜRÜN GÜNCELLE
        // IntegrationHub.Web -> Services -> ApiService.cs içinde

        public async Task<bool> UpdateProductAsync(ProductViewModel model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // JSON yerine MultipartFormData kullanıyoruz (Resim yükleme desteği için)
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

                // Eğer yeni bir resim seçildiyse onu da pakete ekle
                if (model.ImageUpload != null)
                {
                    var fileStream = model.ImageUpload.OpenReadStream();
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(model.ImageUpload.ContentType);
                    content.Add(fileContent, "imageFile", model.ImageUpload.FileName);
                }

                // PUT isteği gönderiyoruz
                var response = await _httpClient.PutAsync($"api/Products/{model.Id}", content);

                // HATA VARSA YAKALA (Casus Kod) 🕵️‍♂️
                if (!response.IsSuccessStatusCode)
                {
                    var hataMesaji = await response.Content.ReadAsStringAsync();
                    throw new Exception($"GÜNCELLEME HATASI: {hataMesaji}");
                }

                return response.IsSuccessStatusCode;
            }
        }

        // 8. ÜRÜN SİL
        public async Task<bool> DeleteProductAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"api/Products/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}