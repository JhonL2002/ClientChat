using ClientChat.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using System.Text;
using System.Text.Json;

namespace ClientChat.Responses
{
    public class LoginService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJSRuntime _js;
        private readonly ILogger<LoginService> _logger;

        public LoginService(IHttpClientFactory httpClientFactory, IJSRuntime js, ILogger<LoginService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _js = js;
            _logger = logger;

        }

        public async Task<bool> OnLoginAsync(LoginDTO model)
        {
            //Serialize the information to be processed
            var jsonContent = new StringContent(JsonSerializer.Serialize(model),
                Encoding.UTF8,
                "application/json"
            );

            //Create the HTTP client using the BackendChat named factory
            var httpClient = _httpClientFactory.CreateClient("BackendChat");

            //Execute a POST request and store the response.
            using HttpResponseMessage response = await httpClient.PostAsync("login", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result != null)
                {
                    await _js.InvokeVoidAsync("sessionStorageHelper.setItem", "jwtToken", result.Token);
                    //Delete log for production scenarios
                    _logger.LogWarning($"**** Token {result.Token} ****");
                    return true;
                }
            }
            _logger.LogError("**** Error to get token ****");
            return false;
        }
    }
}
