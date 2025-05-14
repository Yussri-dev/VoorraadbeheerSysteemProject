using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VoorraadbeheerSysteemProject.Wpf.Models;

namespace VoorraadbeheerSysteemProject.Wpf.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public ApiService(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<List<ProductDTO>> GetProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/product");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<List<ProductDTO>>();
            }
            catch (Exception ex)
            {
                return new List<ProductDTO>();
            }
        }

        public async Task PutProductAsync(ProductDTO product)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/product/{product.ProductId}", product);
                response.EnsureSuccessStatusCode();

                //return await response.Content.ReadFromJsonAsync<ProductDTO>();
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
