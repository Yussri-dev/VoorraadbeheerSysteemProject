namespace VoorraadbeheerSysteemProject.Wpf.Models;

public class SupplierDTO
{
    public int SupplierId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string PhoneNumber1 { get; set; } = string.Empty;

    public string PhoneNumber2 { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Adresse { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Land { get; set; } = string.Empty;

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime DateCreated { get; set; } = DateTime.Now;

    public string ModifiedBy { get; set; } = string.Empty;

    public DateTime DateModified { get; set; } = DateTime.Now;

    public bool IsActivate { get; set; } = true;

    public int SaasClientId { get; set; }

}