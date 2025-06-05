using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VoorraadbeheerSysteemProject.Wpf.Models;

namespace VoorraadbeheerSysteemProject.Wpf.Services.Drawer
{
    class DrawerRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public DrawerRequests(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<CashShiftDTO> PostCashShiftAsync(CashShiftDTO cashShift)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/cashshift", cashShift);
                if (response.IsSuccessStatusCode)
                {
                    var createdCashShift = await response.Content.ReadFromJsonAsync<CashShiftDTO>();
                    return createdCashShift;
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

        public async Task<CashShiftDTO?> GetCashShiftTodayByEmployeeId(string employeeId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/cashshift/drawer/{employeeId}");

                if (response.IsSuccessStatusCode)
                {
                    var cashShift = await response.Content.ReadFromJsonAsync<CashShiftDTO>();

                    return cashShift;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
