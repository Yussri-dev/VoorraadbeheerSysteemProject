using System.ComponentModel.DataAnnotations;

namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class TaxDTO
    {
        public int TaxId { get; set; }

        [Required]
        public decimal TaxRate { get; set; }

    }
}
