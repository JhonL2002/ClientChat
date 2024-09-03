using ClientChat.DTOs;
using ClientChat.DTOs.Chats;
using ClientChat.Responses;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Text;
using System;
using Microsoft.AspNetCore.Mvc;

namespace ClientChat.Services.Chat
{
    public class ChatService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ChatService> _logger;
        private readonly ManageTokenService _manageTokenService;

        public ChatService(IHttpClientFactory httpClientFactory, ILogger<ChatService> logger, ManageTokenService manageTokenService)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _manageTokenService = manageTokenService;
        }

        [BindProperty]
        public IEnumerable<ChatMediaResponse> Messages { get; set; }

        public async Task<MessageResponse> SendMediaAsync(ChatMessageDTO model)
        {
            //Pass the data to API
            using var content = new MultipartFormDataContent
            {
                { new StringContent(model.UserId.ToString()), "UserId" },
                { new StringContent(model.ChatId.ToString()), "ChatId" },
                { new StringContent(model.Text ?? ""), "Text" },
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
                var token = _manageTokenService.GetToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await token);

                //Execute a POST request and store the response.
                using HttpResponseMessage response = await httpClient.PostAsync("send-message", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<MessageResponse>();
                    if (result != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Message sent succesfully!");
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

        public async Task GetMessagesAsync(int chatId, int? lastMessageId, int pageSize = 6)
        {
            //Create the HTTP client using the BackendChat named factory
            var httpClient = _httpClientFactory.CreateClient("ChatClient");

            //Create a new Url with necesary params
            var url = $"{chatId}/messages";
            var queryParams = new List<string>();
            if (lastMessageId.HasValue)
            {
                queryParams.Add($"lastMessageId={lastMessageId.Value}");
            }
            queryParams.Add($"pageSize={pageSize}");

            if (queryParams.Count > 0)
            {
                url += "?" + string.Join("&", queryParams);
            }

            try
            {
                var token = _manageTokenService.GetToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await token);

                //Perform the GET request and store the response
                using HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    using var contentStream = await response.Content.ReadAsStreamAsync();
                    Messages = await JsonSerializer.DeserializeAsync<IEnumerable<ChatMediaResponse>>(contentStream);
                    _logger.LogInformation($"Content fetched successfully! {JsonSerializer.Serialize(Messages)}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception ocurred while fetching messages");
            }
        }
    }
}
