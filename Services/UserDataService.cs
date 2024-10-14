using ClientChat.DTOs;
using ClientChat.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ClientChat.Services
{
    public class UserDataService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LoginService> _logger;
        private readonly ManageTokenService _manageTokenService;
        private readonly JsonSerializerOptions _jsonOptions;
        public UpdateDTO UserData { get; set; }

        public UserDataService(IHttpClientFactory httpClientFactory,
            ILogger<LoginService> logger,
            ManageTokenService manageTokenService,
            JsonSerializerOptions jsonOptions
            )
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _manageTokenService = manageTokenService;
            _jsonOptions = jsonOptions;
        }

        public async Task GetUserDataAsync()
        {
            //Create the HTTP client using the BackendChat named factory
            var httpClient = _httpClientFactory.CreateClient("BackendChat");

            try
            {
                var token = _manageTokenService.GetToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await token);

                //Perform the GET request and store the response
                using HttpResponseMessage response = await httpClient.GetAsync("get-user");

                if (response.IsSuccessStatusCode)
                {
                    using var contentStream = await response.Content.ReadAsStreamAsync();
                    UserData = await JsonSerializer.DeserializeAsync<UpdateDTO>(contentStream, _jsonOptions);
                    _logger.LogInformation($"Content fetched successfully! {JsonSerializer.Serialize(UserData)}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception ocurred while trying to update user");
            }
        }
    }
}
