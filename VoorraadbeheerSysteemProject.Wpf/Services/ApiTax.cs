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

    public class ApiTax
    {
        private readonly HttpClient _httpClient;

        private readonly string _baseUrl;
        public ApiTax(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }
        public async Task<List<TaxDTO>> GetTaxesAsync(int pageNumber, int pageSize)
        {
            var result = await _httpClient.GetFromJsonAsync<List<TaxDTO>>($"api/tax?pageNumber={pageNumber}&pageSize={pageSize}");
            return result ?? new List<TaxDTO>();
        }

        public async Task<int> GetTaxCountAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<int>("api/tax/count");
            return result;
        }

        public async Task<bool> PostTaxAsync(TaxDTO newTax)
        {
            var response = await _httpClient.PostAsJsonAsync("api/tax", newTax);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTaxAsync(int TaxId)
        {
            var response = await _httpClient.DeleteAsync($"api/tax/{TaxId}");
            return response.IsSuccessStatusCode;
        }
    }
}