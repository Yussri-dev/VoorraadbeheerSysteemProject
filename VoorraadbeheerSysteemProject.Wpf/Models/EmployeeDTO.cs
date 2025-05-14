namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        //public string NameComplete => FirstName + " " + LastName;
        public string NameComplete => $"{FirstName} {LastName}";
        public string Role { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int SaasClientId { get; set; }

    }
}
