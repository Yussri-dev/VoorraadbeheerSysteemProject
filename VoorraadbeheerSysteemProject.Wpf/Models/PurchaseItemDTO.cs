namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class PurchaseItemDTO
    {
        public int PurchaseItemId { get; set; }
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; } = 0m;
        public decimal Price { get; set; } = 0m;
        public decimal Discount { get; set; } = decimal.Zero;
        public decimal TaxAmount { get; set; } = decimal.Zero;
        public decimal Total { get; set; } = decimal.Zero;
        public string? ProductName { get; set; }
        public DateTime DateCreated { get; set; }
        public int SaasClientId { get; set; }

    }
}
