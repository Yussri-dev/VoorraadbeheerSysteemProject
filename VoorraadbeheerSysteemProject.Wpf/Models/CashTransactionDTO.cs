using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class CashTransactionDTO
    {
        public int CashTransactionId { get; set; }
        public int CashShiftId { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public decimal Amount { get; set; } = decimal.Zero;
        public DateTime TransactionTime { get; set; } = DateTime.UtcNow;
        public string Description { get; set; } = string.Empty;
        public int SaasClientId { get; set; }
    }
}
