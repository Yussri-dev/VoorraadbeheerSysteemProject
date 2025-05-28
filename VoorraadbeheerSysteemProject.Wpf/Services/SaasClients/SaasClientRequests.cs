using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VoorraadbeheerSysteemProject.Wpf.Helpers;
using VoorraadbeheerSysteemProject.Wpf.Models;

namespace VoorraadbeheerSysteemProject.Wpf.Services.SaasClients
{
    class SaasClientRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public SaasClientRequests(string baseUrl)
        {

            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<bool> RegisterSaasClietnAsync(SaasClientDTO saasClient)
        {
            var response = await _httpClient.PostAsJsonAsync("api/SaasClient", saasClient);
            return response.IsSuccessStatusCode;
        }
    }
}
