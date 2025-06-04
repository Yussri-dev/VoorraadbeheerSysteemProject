using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using VoorraadbeheerSysteemProject.Wpf.Models;

namespace VoorraadbeheerSysteemProject.Wpf.Services.CashRegister
{
    public class CashRegisterRequest
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public CashRegisterRequest(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }


        public async Task<CashShiftDTO?> GetShiftByIdAsync(int cashShiftId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/cashshift/{cashShiftId}");

                if (!response.IsSuccessStatusCode)
                    return null;
                
                var cashShift = await response.Content.ReadFromJsonAsync<CashShiftDTO>();
                return cashShift;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<CashShiftCloseResultDto?> PostEndShift(decimal cashamount, int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/cashshift/{id}", cashamount);
                if(response.IsSuccessStatusCode)
                {
                    var endShift = await response.Content.ReadFromJsonAsync<CashShiftCloseResultDto>();
                    return endShift;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
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
