using System.Net.Http;
using System.Net.Http.Json;
using VoorraadbeheerSysteemProject.Wpf.Models;

namespace VoorraadbeheerSysteemProject.Wpf.Services
{
    public class ApiCustomer
    {

        private readonly HttpClient _httpClient;

        private readonly string _baseUrl;
        public ApiCustomer(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }


        public async Task<List<CustomerDTO>> GetCustomersAsync(int pageNumber, int pageSize)
        {

            var result = await _httpClient.GetFromJsonAsync<List<CustomerDTO>>($"api/customer?pageNumber={pageNumber}&pageSize={pageSize}");
            return result ?? new List<CustomerDTO>();
        }

        public async Task<int> GetCustomerCountAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<int>("api/customer/count");
            return result;
        }

        public async Task<bool> PostCustomerAsync(CustomerDTO newCustomer)
        {
            var response = await _httpClient.PostAsJsonAsync("api/customer", newCustomer);
            return response.IsSuccessStatusCode;
        }

        public async Task UpdateCustomerAsync(CustomerDTO customer)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/customer/{customer.CustomerId}", customer);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> DeleteCustomersAsync(int customerId)
        {
            var response = await _httpClient.DeleteAsync($"api/customer/{customerId}");
            return response.IsSuccessStatusCode;
        }



    }
}




