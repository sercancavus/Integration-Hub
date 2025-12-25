using Microsoft.AspNetCore.Http; // <-- Bu kütüphane IFormFile için ŞART!

namespace IntegrationHub.Web.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Barcode { get; set; }
        public string? SKU { get; set; }
        public decimal SalePrice { get; set; }
        public decimal MarketPrice { get; set; }
        public int StockQuantity { get; set; }

        // Kategori İlişkisi
        public int CategoryId { get; set; }
        public CategoryViewModel? Category { get; set; }

        // --- YENİ EKLENEN KISIMLAR ---

        // 1. Veritabanından gelen resim yolu (Örn: "/images/urun1.jpg") - Göstermek için
        public string? ImageUrl { get; set; }

        // 2. Kullanıcının yükleyeceği dosya - API'ye göndermek için
        public IFormFile? ImageUpload { get; set; }
    }
}