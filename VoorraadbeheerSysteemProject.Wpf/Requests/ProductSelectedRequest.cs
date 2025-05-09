using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoorraadbeheerSysteemProject.Wpf.Requests
{
    public class ProductSelectedRequest
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
    }
}
