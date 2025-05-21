using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Automation.Peers;
using VoorraadbeheerSysteemProject.Wpf.Models;

namespace VoorraadbeheerSysteemProject.Wpf.Services.Sales
{
    public class SalesRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        public SalesRequests(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<int> GetSalesCountAsync()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync("api/sale/count");

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

        public async Task<decimal> GetSumByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                string formattedStartDate = startDate.ToString("yyyy-MM-dd");
                string formattedEndDate = endDate.ToString("yyyy-MM-dd");

                HttpResponseMessage responseRequest = await _httpClient.GetAsync(
                    $"api/sale/SalesAmount?startDate={formattedStartDate}&endDate={formattedEndDate}");

                if (!responseRequest.IsSuccessStatusCode)
                {
                    return 0;
                }

                string responseJson = await responseRequest.Content.ReadAsStringAsync();

                if (decimal.TryParse(responseJson, out decimal count))
                {
                    return count;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"API request error: {ex.Message}");
                return 0;
            }
        }

        public async Task<IEnumerable<MonthlySummaryDTO>> GetMonthlySummaryAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                string formattedStartDate = startDate.ToString("yyyy-MM-dd");
                string formattedEndDate = endDate.ToString("yyyy-MM-dd");

                HttpResponseMessage response = await _httpClient.GetAsync(
                    $"api/sale/monthly-summary?startDate={formattedStartDate}&endDate={formattedEndDate}");

                if (!response.IsSuccessStatusCode)
                {
                    return new List<MonthlySummaryDTO>();
                }

                string responseJson = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var monthlySummaries = JsonSerializer.Deserialize<List<MonthlySummaryDTO>>(responseJson, options);

                return monthlySummaries ?? new List<MonthlySummaryDTO>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"API request error: {ex.Message}");
                return new List<MonthlySummaryDTO>();
            }
        }


        public async Task<List<SaleFlatDTO>?> GetSaleFlatByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                string formattedStartDate = HttpUtility.HtmlEncode(startDate.ToString("dd/MM/yyyy"));
                string formattedEndDate = HttpUtility.HtmlEncode(endDate.ToString("dd/MM/yyyy"));

                HttpResponseMessage response = await _httpClient.GetAsync(
                    $"api/sale/allsale?startDate={formattedStartDate}&endDate={formattedEndDate}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var saleFlat = await response.Content.ReadFromJsonAsync<List<SaleFlatDTO>>();
                return saleFlat;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<SaleDTO?> PostSaleAsync(SaleDTO sale)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/sale", sale);
                if (response.IsSuccessStatusCode)
                {
                    var createdSale = await response.Content.ReadFromJsonAsync<SaleDTO>();
                    return createdSale;
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

        //PostSaleItemAsync
        public async Task<SaleItemDTO?> PostSaleItemAsync(SaleItemDTO saleItem)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/saleItem", saleItem);
                if (response.IsSuccessStatusCode)
                {
                    var createdSaleItem = await response.Content.ReadFromJsonAsync<SaleItemDTO>();
                    return createdSaleItem;
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

