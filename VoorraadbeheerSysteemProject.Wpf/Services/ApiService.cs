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
                //var response = await _httpClient.GetAsync("api/product");
                var response = await _httpClient.GetAsync("api/product?pageNumber=1&pageSize=100"); //pageNumber pageSize
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

        public async Task PostProductAsync(ProductDTO product)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/product", product);
                response.EnsureSuccessStatusCode();
                
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/category");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<CategoryDTO>>();
            }
            catch (Exception ex)
            {
                return new List<CategoryDTO>();
            }
        }

        public async Task<List<TaxDTO>> GetTaxRatesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/Tax");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<TaxDTO>>();
            }
            catch (Exception ex)
            {
                return new List<TaxDTO>();
            }
        }

        public async Task<List<ShelfDTO>> GetShelfsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/shelf");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<ShelfDTO>>();
            }
            catch (Exception ex)
            {
                return new List<ShelfDTO>();
            }
        }

    }
}
