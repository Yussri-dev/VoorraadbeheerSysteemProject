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
                _httpClient.BaseAddress = new Uri("https://eafe-2a02-2c40-270-2029-ce-31dc-60b-8f26.ngrok-free.app/");
            }

            public async Task<List<CategoryDTO>> GetCategoriesAsync()
            {
                var result = await _httpClient.GetFromJsonAsync<List<CategoryDTO>>("api/category?pageNumber=1&pagesize=200");
                return result ?? new List<CategoryDTO>();
            }

        public async Task<bool> PostCategoryAsync(CategoryDTO newCategory)
        {
            var response = await _httpClient.PostAsJsonAsync("api/category", newCategory);
            return response.IsSuccessStatusCode;
        }
    }
    }


