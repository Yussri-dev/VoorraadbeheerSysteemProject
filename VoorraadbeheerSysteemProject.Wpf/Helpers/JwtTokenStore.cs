using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoorraadbeheerSysteemProject.Wpf.Helpers
{
    public static class JwtTokenStore
    {
        private static string? _token;

        public static string? Token
        {
            get => _token;
            set => _token = value;
        }

        public static bool IsTokenValid()
        {
            return !string.IsNullOrEmpty(_token);
        }

        public static void ClearToken()
        {
            _token = null;
        }
    }

}
