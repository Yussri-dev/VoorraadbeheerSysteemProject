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

        public ApiSupplier()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://49f9-2a02-2c40-270-2029-15bc-2224-879c-3b8.ngrok-free.app/");
        }

        public async Task<List<SupplierDTO>> GetSuppliersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<SupplierDTO>>("api/supplier");
        }
    }
}
