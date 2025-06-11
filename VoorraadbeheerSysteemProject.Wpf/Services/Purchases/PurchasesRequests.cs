using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VoorraadbeheerSysteemProject.Wpf.Models;

namespace VoorraadbeheerSysteemProject.Wpf.Services.Purchases
{
    class PurchasesRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public PurchasesRequests(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<int> GetPurchasesCountAsync()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync("api/purchase/count");

            if (!responseMessage.IsSuccessStatusCode)
            {
                return 0;
            }

            string responseCount = await responseMessage.Content.ReadAsStringAsync();
            if (int.TryParse(responseCount, out int count))
            {
                return count;
            }
            return 0;
        }

        public async Task<decimal> GetSumPurchaseByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                string formattedStartDate = startDate.ToString("yyyy-MM-dd");
                string formattedEndDate = endDate.ToString("yyyy-MM-dd");

                HttpResponseMessage responseRequest = await _httpClient.GetAsync(
                    $"api/purchase/PurchasesAmount?startDate={formattedStartDate}&endDate={formattedEndDate}");

                if (!responseRequest.IsSuccessStatusCode)
                {
                    return 0;
                }

                string responseJson = await responseRequest.Content.ReadAsStringAsync();

                if (decimal.TryParse(responseJson,CultureInfo.InvariantCulture, out decimal amountPurchase))
                {
                    return amountPurchase;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"API request error: {ex.Message}");
                return 0;
            }
        }

        public async Task<List<PurchaseFlatDTO>> GetPurchaseFlatByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                string encodedStartDate = HttpUtility.HtmlEncode(startDate.ToString("dd/MM/yyyy"));
                string encodedEndDate = HttpUtility.HtmlEncode(endDate.ToString("dd/MM/yyyy"));

                var response = await _httpClient.GetAsync($"api/purchase/allpurchase?userid={UserSession.IdUSer}&startdate={encodedStartDate}&enddate={encodedEndDate}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<PurchaseFlatDTO>>();

            }
            catch (Exception ex)
            {
                return new List<PurchaseFlatDTO>();
            }
        }

        public async Task<PurchaseDTO?> PostPurchaseAsync(PurchaseDTO purchase)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/purchase", purchase);
                if (response.IsSuccessStatusCode)
                {
                    var createdPurchase = await response.Content.ReadFromJsonAsync<PurchaseDTO>();
                    return createdPurchase;
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

        ////PostSaleItemAsync
        public async Task<PurchaseItemDTO?> PostPurchaseItemAsync(PurchaseItemDTO purchaseItem)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/purchaseItem", purchaseItem);
                if (response.IsSuccessStatusCode)
                {
                    var createdPurchaseItem = await response.Content.ReadFromJsonAsync<PurchaseItemDTO>();
                    return createdPurchaseItem;
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
