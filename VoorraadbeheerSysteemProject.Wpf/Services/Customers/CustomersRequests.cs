using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VoorraadbeheerSysteemProject.Wpf.Models;

namespace VoorraadbeheerSysteemProject.Wpf.Services.Customers
{
    partial class CustomersRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public CustomersRequests(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<int> GetCustomersCountAsync()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync("api/customer/count");
            if (!responseMessage.IsSuccessStatusCode)
            {
                return 0;
            }

            string responseCount = await responseMessage.Content.ReadAsStringAsync();
            if (int.TryParse(responseCount, out int count))
            {
                return count;
            }
            return 0;
        }

        public async Task<List<CustomerDTO>> GetCustomers()
        {
            var result = await _httpClient.GetFromJsonAsync<List<CustomerDTO>>($"api/Customer");
            return result ?? new List<CustomerDTO>();
        }

    }
}
