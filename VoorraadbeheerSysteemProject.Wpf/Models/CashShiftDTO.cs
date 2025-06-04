using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class CashShiftDTO
    {
        public int CashShiftId { get; set; }
        public DateTime ShiftDate { get; set; }
        public DateTime ShiftStart { get; set; }
        public DateTime? ShiftEnd { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalRefunds { get; set; }
        public decimal CashIn { get; set; }
        public decimal CashOut { get; set; }
        public decimal DrawerBalance { get; set; }
        public DateTime DateCreated { get; set; }
        public int CashRegisterId { get; set; }
        public int EmployeeId { get; set; }
        public string? NameComplete { get; set; }

        public int SaasClientId { get; set; }

    }
}
