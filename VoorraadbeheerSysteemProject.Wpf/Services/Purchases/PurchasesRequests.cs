using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
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
