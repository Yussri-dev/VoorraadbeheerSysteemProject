using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VoorraadbeheerSysteemProject.Wpf.Models;

namespace VoorraadbeheerSysteemProject.Wpf.Services.Sales
{
    public class SalesRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public SalesRequests(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<SaleDTO?> PostSaleAsync(SaleDTO sale)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/sale", sale);
                if (response.IsSuccessStatusCode)
                {
                    var createdSale = await response.Content.ReadFromJsonAsync<SaleDTO>();
                    return createdSale;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

