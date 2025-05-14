namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class SaleItemDTO
    {
        public int SaleItemId { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; } = 0m;
        public decimal Price { get; set; } = 0m;
        public decimal ProfitMarge { get; set; } = 0m;
        public decimal PurchasePrice { get; set; } = 0m;
        public decimal Discount { get; set; } = decimal.Zero;
        public decimal TaxAmount { get; set; } = decimal.Zero;
        public decimal Total { get; set; } = decimal.Zero;
        public string? ProductName { get; set; }
        public DateTime DateCreated { get; set; }
        public int SaasClientId { get; set; }

        //public ProductDTO Product { get; set; } // Include ProductDTO


    }
}
