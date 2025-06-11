using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VoorraadbeheerSysteemProject.Wpf.Helpers
{
    public static class JwtTokenHelper
    {
        public static void SetJwtToken(HttpClient httpClient, string jwtToken)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwtToken);
        }
    }
}
