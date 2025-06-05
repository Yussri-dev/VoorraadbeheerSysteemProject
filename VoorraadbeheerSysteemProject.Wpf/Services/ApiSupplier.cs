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
    public class ApiSupplier
    {

        private readonly HttpClient _httpClient;

        private readonly string _baseUrl;
        public ApiSupplier(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }


        public async Task<List<SupplierDTO>> GetSuppliersAsync(int pageNumber, int pageSize)
        {

            var result = await _httpClient.GetFromJsonAsync<List<SupplierDTO>>($"api/supplier?pageNumber={pageNumber}&pageSize={pageSize}");
            return result ?? new List<SupplierDTO>();
        }

        public async Task<int> GetSupplierCountAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<int>("api/supplier/count");
            return result;
        }

        public async Task<bool> PostSupplierAsync(SupplierDTO newSupplier)
        {
            var response = await _httpClient.PostAsJsonAsync("api/supplier", newSupplier);
            return response.IsSuccessStatusCode;
        }




        public async Task<bool> DeleteSuppliersAsync(int supplierId)
        {
            var response = await _httpClient.DeleteAsync($"api/supplier/{supplierId}");
            return response.IsSuccessStatusCode;
        }

        

    }
}




