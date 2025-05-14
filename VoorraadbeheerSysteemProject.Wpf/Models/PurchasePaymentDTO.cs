namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class PurchasePaymentDTO
    {
        public int PurchasePaymentId { get; set; }
        public int PurchaseId { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; } = decimal.Zero;
        public string PaymentType { get; set; } = string.Empty;
        public int SaasClientId { get; set; }
    }
}
