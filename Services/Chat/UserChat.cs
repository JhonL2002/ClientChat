using ClientChat.DTOs.Chats;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ClientChat.Services.Chat
{
    public class UserChat
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ManageTokenService _manageTokenService;
        private readonly ILogger<UserChat> _logger;

        public UserChat(IHttpClientFactory httpClientFactory, ManageTokenService manageTokenService, ILogger<UserChat> logger)
        {
            _httpClientFactory = httpClientFactory;
            _manageTokenService = manageTokenService;
            _logger = logger;
        }

        [BindProperty]
        public IEnumerable<ChatDTO> Chats { get; set; }

        public async Task GetUserChatsAsync()
        {
            //Create the HTTP client using the BackendChat named factory
            var httpClient = _httpClientFactory.CreateClient("ChatClient");

            try
            {
                var token = _manageTokenService.GetToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await token);

                //Perform the GET request and store the response
                using HttpResponseMessage response = await httpClient.GetAsync("user-chats");

                if (response.IsSuccessStatusCode)
                {
                    using var contentStream = await response.Content.ReadAsStreamAsync();
                    Chats = await JsonSerializer.DeserializeAsync<IEnumerable<ChatDTO>>(contentStream);
                    _logger.LogInformation($"Content fetched successfully! {JsonSerializer.Serialize(Chats)}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception ocurred while fetching chats");
            }
        }
    }
}
