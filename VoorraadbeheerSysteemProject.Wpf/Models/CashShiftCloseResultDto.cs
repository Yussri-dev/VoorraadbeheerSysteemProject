using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class CashShiftCloseResultDto
    {
        public decimal Expected { get; set; }
        public decimal Actual { get; set; }
        public decimal Difference => Actual - Expected;
    }
}
