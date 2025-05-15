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
                _httpClient.BaseAddress = new Uri("https://7ec3-2a02-2c40-270-2029-ddf8-5e41-2ecd-60cd.ngrok-free.app");
            }

            public async Task<List<CategoryDTO>> GetCategoriesAsync()
            {
                var result = await _httpClient.GetFromJsonAsync<List<CategoryDTO>>("api/category");
                return result ?? new List<CategoryDTO>();
            }
        }
    }


