namespace IntegrationHub.Web.Models // YENİ İSİM (Noktasız)
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal SalePrice { get; set; }
        public decimal MarketPrice { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; } // Seçilen Kategori ID'si
        public CategoryViewModel? Category { get; set; } // Listede ismini göstermek için
    }
}