namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class ReturnItemDTO
    {
        public int ReturnItemId { get; set; }
        public int ReturnId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; } = 0m;
        public decimal RefundAmount { get; set; } = 0m;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public string ProductName { get; set; }

        public int SaasClientId { get; set; }

    }
}
