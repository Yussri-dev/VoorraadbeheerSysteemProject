namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal CreditLimit { get; set; } = 1000m;
        public decimal AccountBalance { get; set; } = 0m;
        public string PhoneNumber1 { get; set; } = string.Empty;
        public string PhoneNumber2 { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Land { get; set; } = string.Empty;
        public bool IsActivate { get; set; } = true;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime? DateModified { get; set; }
        public decimal AmountPaid { get; set; } = 0m;
        public decimal BalancePaid => AccountBalance - AmountPaid;
        public int SaasClientId { get; set; }

    }
}
