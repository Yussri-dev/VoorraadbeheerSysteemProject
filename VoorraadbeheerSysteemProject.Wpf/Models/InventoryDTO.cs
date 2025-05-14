namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class InventoryDTO
    {
        public int InventoryId { get; set; }
        public int LocationId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; } = 0;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTime DateModified { get; set; } = DateTime.Now;
        public int SaasClientId { get; set; }
    }
}
