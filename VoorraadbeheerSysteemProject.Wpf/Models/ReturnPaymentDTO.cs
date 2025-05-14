namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class ReturnPaymentDTO
    {
        public int ReturnPaymentId { get; set; }
        public int ReturnId { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; } = decimal.Zero;
        public string PaymentType { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public int SaasClientId { get; set; }
    }
}
