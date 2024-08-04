using ClientChat.DTOs;
using ClientChat.Services;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ClientChat.Responses
{
    public class LoginService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ManageTokenService _tokenService;
        private readonly ILogger<LoginService> _logger;

        public LoginService(IHttpClientFactory httpClientFactory, ManageTokenService tokenService, ILogger<LoginService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<LoginResponse> OnLoginAsync(LoginDTO model)
        {
            //Serialize the information to be processed
            var jsonContent = new StringContent(JsonSerializer.Serialize(model),
                Encoding.UTF8,
                "application/json"
            );

            //Create the HTTP client using the BackendChat named factory
            var httpClient = _httpClientFactory.CreateClient("BackendChat");

            try
            {
                //Execute a POST request and store the response.
                using HttpResponseMessage response = await httpClient.PostAsync("login", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    if (result != null)
                    {
                        var token = result.Token;
                        //Save the token in ManageTokenService
                        await _tokenService.SetToken(token);

                        //Delete log for production scenarios
                        _logger.LogInformation($"**** Token {token} ****");
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        return result;
                    }
                    else
                    {
                        _logger.LogWarning("Received null result from login response");
                    }
                }
                else
                {
                    _logger.LogError($"Failded to login. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception ocurred while trying to login");
            }
            return null!;
        }
    }
}
