namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class PurchaseDTO
    {
        public int PurchaseId { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
        public int SupplierId { get; set; }
        public string UserId { get; set; }
        public decimal TvaAmount { get; set; } = 0m;
        public decimal TotalAmount { get; set; } = 0m;
        public decimal AmountPaid { get; set; } = 0m;
        public decimal OutstandingBalance => TotalAmount - AmountPaid;
        public string? SupplierName { get; set; }

        public int SaasClientId { get; set; }
    }
}
