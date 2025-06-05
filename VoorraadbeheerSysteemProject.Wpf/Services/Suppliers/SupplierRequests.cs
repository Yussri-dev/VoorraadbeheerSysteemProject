using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VoorraadbeheerSysteemProject.Wpf.Models;

namespace VoorraadbeheerSysteemProject.Wpf.Services.Suppliers
{
    class SupplierRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public SupplierRequests(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<List<SupplierDTO>> GetSuppliers()
        {
            var result = await _httpClient.GetFromJsonAsync<List<SupplierDTO>>($"api/Supplier");
            return result ?? new List<SupplierDTO>();
        }

        public async Task<int> GetSuppliersCountAsync()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync("api/supplier/count");
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
    }
}
