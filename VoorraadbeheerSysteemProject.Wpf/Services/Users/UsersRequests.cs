using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VoorraadbeheerSysteemProject.Wpf.Models;

namespace VoorraadbeheerSysteemProject.Wpf.Services.Users
{
    public class UsersRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public UsersRequests(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }


        public async Task<bool> LoginAsync(string userName, string password)
        {
            var loginRequest = new LoginDto
            {
                Email = userName,
                Password = password
            };
            var response = await _httpClient.PostAsJsonAsync("login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
