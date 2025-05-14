namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class LocationDTO
    {
        public int LocationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber1 { get; set; } = string.Empty;
        public string PhoneNumber2 { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Adresse { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Land { get; set; } = string.Empty;
        public string ModifiedBy { get; set; }
        public bool IsActivated { get; set; }
        public int SaasClientId { get; set; }

    }
}
