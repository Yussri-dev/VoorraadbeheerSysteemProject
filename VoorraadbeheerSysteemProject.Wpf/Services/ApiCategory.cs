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

    public class ApiCategory
    {
        private readonly HttpClient _httpClient;

        private readonly string _baseUrl;
        public ApiCategory(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }
        public async Task<List<CategoryDTO>> GetCategoriesAsync(int pageNumber, int pageSize)
        {
            var result = await _httpClient.GetFromJsonAsync<List<CategoryDTO>>($"api/category?pageNumber={pageNumber}&pageSize={pageSize}");
            return result ?? new List<CategoryDTO>();
        }

        public async Task<int> GetCategoryCountAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<int>("api/category/count");
            return result; 
        }

        public async Task<bool> PostCategoryAsync(CategoryDTO newCategory)
        {
            var response = await _httpClient.PostAsJsonAsync("api/category", newCategory);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var response = await _httpClient.DeleteAsync($"api/category/{categoryId}");
            return response.IsSuccessStatusCode;
        }
    }

}


