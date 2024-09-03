using ClientChat.DTOs.Chats;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using ClientChat.Services.Chat;
using ClientChat.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ClientChat.Services.Group
{
    public class ManageGroupService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ManageGroupService> _logger;
        private readonly ManageTokenService _manageTokenService;

        public ManageGroupService(IHttpClientFactory httpClientFactory, ManageTokenService manageTokenService, ILogger<ManageGroupService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _manageTokenService = manageTokenService;
            _logger = logger;

        }

        [BindProperty]
        public IEnumerable<GroupResponse> Groups { get; set; }

        public async Task<bool> CreateGroupAsync(GroupDTO model)
        {

            //Serialize the information to be processed
            var jsonContent = new StringContent(JsonSerializer.Serialize(model),
                Encoding.UTF8,
                "application/json"
            );

            //Create the HTTP client using the BackendChat named factory
            var httpClient = _httpClientFactory.CreateClient("ChatClient");

            try
            {
                var token = _manageTokenService.GetToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await token);
                //Execute a POST request and store the response.
                using HttpResponseMessage response = await httpClient.PostAsync("create-group", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Group created successfully");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failded to create group. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception ocurred while trying to create group");
            }

            return false;
        }

        public async Task GetAllGroupsAsync()
        {
            //Create the HTTP client using the BackendChat named factory
            var httpClient = _httpClientFactory.CreateClient("ChatClient");

            try
            {
                var token = _manageTokenService.GetToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await token);

                //Perform the GET request and store the response
                using HttpResponseMessage response = await httpClient.GetAsync("get-groups");

                if (response.IsSuccessStatusCode)
                {
                    using var contentStream = await response.Content.ReadAsStreamAsync();
                    Groups = await JsonSerializer.DeserializeAsync<IEnumerable<GroupResponse>>(contentStream);
                    _logger.LogInformation($"Content fetched successfully! {JsonSerializer.Serialize(Groups)}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception ocurred while fetching messages");
            }
        }
    }
}
