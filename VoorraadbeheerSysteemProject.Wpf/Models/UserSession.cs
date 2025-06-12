using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoorraadbeheerSysteemProject.Wpf.Models
{
    public static class UserSession
    {
        public static string? Email { get; set; }
        public static string? Token { get; set; }
        public static string? IdUSer { get; set; }
        public static int IdSaasClient { get; set; }
        public static List<string> Roles { get; set; }

        public static void Clear()
        {
            Email = null;
            Token = null;
            IdUSer = null;
            Roles.Clear();
        }
    }
}
