using System.Text.Json.Serialization;

namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int SaasClientId { get; set; }

    }
}
