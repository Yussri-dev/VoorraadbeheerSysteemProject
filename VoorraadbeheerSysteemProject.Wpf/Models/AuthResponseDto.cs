using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string IdUser { get; set; }
        public int IdSaasClient { get; set; }
        public List<string> Roles { get; set; }
        public string ErrorMessage { get; set; }
    }
}
