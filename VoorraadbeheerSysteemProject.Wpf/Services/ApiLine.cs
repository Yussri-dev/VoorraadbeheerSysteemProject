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
   
        public class ApiLine
        {
        private readonly HttpClient _httpClient;

        private readonly string _baseUrl;
        public ApiLine(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }
        public async Task<List<LineDTO>> GetLinesAsync(int pageNumber, int pageSize)
        {
            var result = await _httpClient.GetFromJsonAsync<List<LineDTO>>($"api/line?pageNumber={pageNumber}&pageSize={pageSize}");
            return result ?? new List<LineDTO>();
        }

        public async Task<int> GetLineCountAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<int>("api/line/count");
            return result;
        }

        public async Task<bool> PostLineAsync(LineDTO newLine)
        {
            var response = await _httpClient.PostAsJsonAsync("api/line", newLine);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteLineAsync(int LineId)
        {
            var response = await _httpClient.DeleteAsync($"api/tax/{LineId}");
            return response.IsSuccessStatusCode;
        }
    }


    }
