using System.ComponentModel.DataAnnotations;

namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class RegisterDto
    {
        public int SaasClientId { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [MinLength(6)]
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public List<string> Roles { get; set; }


    }
}
