using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class SaleDTO
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public int CustomerId { get; set; }
        public string UserId { get; set; }
        public decimal TvaAmount { get; set; } = 0m;
        public decimal TotalAmount { get; set; } = 0m;
        public decimal AmountPaid { get; set; } = 0m;
        public decimal OutstandingBalance => TotalAmount - AmountPaid;
        public decimal DiscountPercentage { get; set; } = decimal.Zero;
        public string? CustomerName { get; set; }
        public int SaasClientId { get; set; }
    }
}
