using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class CashRegisterDTO
    {
        public int CashRegisterId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int LocationId { get; set; }
        public int EmployeeId { get; set; }
        public string? NameComplete { get; set; }
        public string? NameLocation { get; set; }
        public int SaasClientId { get; set; }
    }
}
