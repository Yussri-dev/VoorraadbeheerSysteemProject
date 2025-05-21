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

        public ApiCategory()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://0d08-2a02-2c40-270-2029-d0df-40-de76-7aec.ngrok-free.app/");
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


