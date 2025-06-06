using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using VoorraadbeheerSysteemProject.Wpf.Helpers;
using VoorraadbeheerSysteemProject.Wpf.Models;

namespace VoorraadbeheerSysteemProject.Wpf.Services.Users
{
    public class UsersRequests
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public UsersRequests(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", JwtTokenStore.Token);
        }


        public bool IsTokenExpired(string token)
        {
            //i have used Jwt Security Package 
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(token))
                return true;

            var jwtToken = handler.ReadJwtToken(token);

            var expiry = jwtToken.ValidTo; // UTC

            return expiry < DateTime.UtcNow;
        }

        public Task<bool> ValidateTokenAsync(string token)
        {
            return Task.FromResult(!IsTokenExpired(token));
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);
                if (response.IsSuccessStatusCode)
                {
                    var resultString = await response.Content.ReadAsStringAsync();

                    var registerResponse = JsonSerializer.Deserialize<AuthResponseDto>(resultString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return registerResponse;
                }

                return new AuthResponseDto { ErrorMessage = "Registration Failed" };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto { ErrorMessage = "Registration Failed" };

            }
        }
        public async Task<AuthResult?> LoginAsync(string userName, string password)
        {
            var loginRequest = new LoginDto { Email = userName, Password = password };
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

                if (authResponse != null && authResponse.IsSuccess)
                {
                    var saas = authResponse.IdSaasClient;
                    return new AuthResult
                    {
                        Token = authResponse.Token,
                        Email = authResponse.UserName,
                        IdUser = authResponse.IdUser,
                        IdSaasClient = authResponse.IdSaasClient
                    };
                }

                else
                {
                    Console.WriteLine($"Authentication failed: {authResponse?.ErrorMessage}");
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Login HTTP error: {response.StatusCode} - {errorContent}");
            }
            return null;

        }

    }
}
