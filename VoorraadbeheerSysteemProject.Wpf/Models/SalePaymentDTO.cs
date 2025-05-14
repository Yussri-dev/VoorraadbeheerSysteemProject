namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class SalePaymentDTO
    {
        public int SalePaymentId { get; set; }
        public int SaleId { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; } = decimal.Zero;
        public string PaymentType { get; set; } = string.Empty;
        public int SaasClientId { get; set; }
    }
}
