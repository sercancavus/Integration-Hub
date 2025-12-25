using IntegrationHub.Web.Models;

namespace IntegrationHub.Web.Services
{
    public interface IApiService
    {
        // --- AUTH İŞLEMLERİ (Eksikti, geri eklendi) ---
        Task<bool> RegisterAsync(RegisterModel model);
        Task<string> LoginAsync(LoginModel model);

        // --- KATEGORİ İŞLEMLERİ ---
        Task<List<CategoryViewModel>> GetCategoriesAsync(string token);

        // --- ÜRÜN İŞLEMLERİ ---
        // İsim düzeltildi: GetAllProductsAsync -> GetProductsAsync
        Task<List<ProductViewModel>> GetProductsAsync(string token);

        // İsim düzeltildi: CreateProductAsync -> AddProductAsync
        Task<bool> AddProductAsync(ProductViewModel model, string token);

        Task<ProductViewModel> GetProductByIdAsync(int id, string token);

        Task<bool> UpdateProductAsync(ProductViewModel model, string token);

        Task<bool> DeleteProductAsync(int id, string token);
    }
}