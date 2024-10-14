using ClientChat.DTOs;
using ClientChat.Responses;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ClientChat.Services
{
    public class UpdateService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<UpdateService> _logger;
        private readonly ManageTokenService _manageTokenService;
        private readonly JsonSerializerOptions _jsonOptions;

        public UpdateService(IHttpClientFactory httpClientFactory,
            ILogger<UpdateService> logger,
            ManageTokenService manageTokenService,
            JsonSerializerOptions jsonOptions)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _manageTokenService = manageTokenService;
            _jsonOptions = jsonOptions;
        }

        public async Task<SuccessResponse> OnUpdateAsync(UpdateDTO model)
        {
            //Pass the data to API
            using var content = new MultipartFormDataContent
            {
                { new StringContent(model.FirstName), "FirstName" },
                { new StringContent(model.LastName), "LastName" },
                { new StringContent(model.DOB.ToString()!), "DOB" },
                { new StringContent(model.Nickname), "Nickname" },
                { new StringContent(model.Email), "Email" },
            };

            if (model.ProfilePicture != null)
            {
                var fileStream = model.ProfilePicture.OpenReadStream();
                var fileContent = new StreamContent(fileStream);
                content.Add(fileContent, "ProfilePicture", model.ProfilePicture.Name);
            }


            //Create the HTTP client using the BackendChat named factory
            var httpClient = _httpClientFactory.CreateClient("BackendChat");

            try
            {
                var token = _manageTokenService.GetToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await token);

                //Execute a PUT request and store the response.
                using HttpResponseMessage response = await httpClient.PutAsync("update-user", content);
                if (response.IsSuccessStatusCode)
                {
                    
                    _logger.LogInformation("User updated succesfully!");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failded to update user. Status code: {response.StatusCode}, Error: {errorContent}");
                    return new SuccessResponse { IsSuccess = false };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception ocurred while trying to update user");
            }

            return new SuccessResponse { IsSuccess = true };
        }
    }
}
