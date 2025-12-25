using IntegrationHub.Web.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace IntegrationHub.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // 1. GİRİŞ METODU (LoginAsync)
        public async Task<string> LoginAsync(string username, string password)
        {
            var loginData = new { Username = username, Password = password };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            // API adresine dikkat (Program.cs'deki BaseAddress'in sonuna eklenir)
            var response = await _httpClient.PostAsync("api/Auth/login", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(responseString);
                return result.token;
            }
            return null;
        }

        // 2. ÜRÜN ÇEKME METODU (GetProductsAsync)
        public async Task<List<ProductViewModel>> GetProductsAsync(string token)
        {
            // 1. Token'ı ekle
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // 2. İsteği at
            var response = await _httpClient.GetAsync("api/Products");

            // 3. HATA VARSA FIRLAT (Değişen kısım burası)
            if (!response.IsSuccessStatusCode)
            {
                // Hatayı okuyalım: 401 Unauthorized mı? 404 Not Found mı?
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Hatası! Kod: {response.StatusCode} - Detay: {errorContent}");
            }

            // 4. Başarılıysa çevir
            var jsonData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ProductViewModel>>(jsonData);
        }

        public async Task<bool> AddProductAsync(ProductViewModel product, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

            // API'deki Post metoduna istek atıyoruz
            var response = await _httpClient.PostAsync("api/Products", jsonContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // API adresine ID'yi ekleyerek gönderiyoruz (örn: api/Products/5)
            var response = await _httpClient.DeleteAsync($"api/Products/{id}");

            return response.IsSuccessStatusCode;
        }

        // 1. Tek ürünü çekmek için
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

        // 2. Güncellemek için
        public async Task<bool> UpdateProductAsync(ProductViewModel product, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

            // PutAsync kullanıyoruz
            var response = await _httpClient.PutAsync("api/Products", jsonContent);

            return response.IsSuccessStatusCode;
        }
        public async Task<List<CategoryViewModel>> GetCategoriesAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("api/Categories");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CategoryViewModel>>(jsonData);
            }
            return new List<CategoryViewModel>();
        }
    }
}