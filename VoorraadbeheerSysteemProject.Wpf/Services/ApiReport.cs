using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VoorraadbeheerSysteemProject.Wpf.Models;

namespace VoorraadbeheerSysteemProject.Wpf.Services
{

    public class ApiReport
    {
        private readonly HttpClient _httpClient;

        private readonly string _baseUrl;
        public ApiReport(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }
        public async Task<List<ProductDTO>> GetReportsAsync(int pageNumber, int pageSize)
        {
            var result = await _httpClient.GetFromJsonAsync<List<ProductDTO>>($"api/product?pageNumber={pageNumber}&pageSize={pageSize}");
            return result ?? new List<ProductDTO>();
        }

        public async Task<int> GetProductCountAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<int>("api/product/count");
            return result;
        }
    }

}
