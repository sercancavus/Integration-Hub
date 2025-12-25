using IntegrationHub.Web.Models; // LoginDto ve RegisterDto burada olmalı
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegrationHub.Web.Services
{
    public interface IApiService
    {
        // --- AUTH ---
        Task<string> LoginAsync(LoginDto loginDto);
        Task<bool> RegisterAsync(RegisterDto registerDto);

        // --- PRODUCTS (ÜRÜNLER) ---
        Task<List<ProductViewModel>> GetProductsAsync(string token = null);
        Task<ProductViewModel> GetProductByIdAsync(int id, string token);
        Task<bool> AddProductAsync(ProductViewModel model, string token);
        Task<bool> UpdateProductAsync(ProductViewModel model, string token);
        Task<bool> DeleteProductAsync(int id, string token);

        // --- KATEGORİLER ---
        Task<List<CategoryViewModel>> GetCategoriesAsync(string token);
        Task<CategoryViewModel> GetCategoryByIdAsync(int id, string token);
        Task<bool> AddCategoryAsync(CategoryViewModel model, string token);
        Task<bool> UpdateCategoryAsync(CategoryViewModel model, string token);
        Task<bool> DeleteCategoryAsync(int id, string token);
    }
}