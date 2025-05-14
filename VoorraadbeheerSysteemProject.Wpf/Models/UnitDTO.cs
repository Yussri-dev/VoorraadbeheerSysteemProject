using System.ComponentModel.DataAnnotations;

namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class UnitDTO
    {
        public int UnitId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

    }
}
