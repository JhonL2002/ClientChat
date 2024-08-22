using ClientChat.DTOs;
using ClientChat.Responses;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http;

namespace ClientChat.Services.Chat
{
    public class ChatService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ChatService> _logger;

        public ChatService(IHttpClientFactory httpClientFactory, ILogger<ChatService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<ChatMediaResponse> SendMediaAsync(ChatMedia model)
        {
            //Pass the data to API
            using var content = new MultipartFormDataContent
            {
                { new StringContent(model.User), "User" },
            };

            if (model.File != null)
            {
                var fileStream = model.File.OpenReadStream();
                var fileContent = new StreamContent(fileStream);
                content.Add(fileContent, "File", model.File.Name);

            }

            //Create the HTTP client using the BackendChat named factory
            var httpClient = _httpClientFactory.CreateClient("ChatClient");

            try
            {
                //Execute a POST request and store the response.
                using HttpResponseMessage response = await httpClient.PostAsync("sendmedia", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ChatMediaResponse>();
                    if (result != null)
                    {
                        return result;
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failded to send message. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception ocurred while trying to send message");
            }

            return null!;

        }
    }
}
