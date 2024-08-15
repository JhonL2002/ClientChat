using ClientChat.DTOs;
using ClientChat.Responses;
using System.Text.Json;
using System.Text;

namespace ClientChat.Services
{
    public class RegistrationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LoginService> _logger;

        public RegistrationService(IHttpClientFactory httpClientFactory, ILogger<LoginService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task OnRegisterAsync(RegisterDTO model)
        {
            //Pass the data to API
            using var content = new MultipartFormDataContent
            {
                { new StringContent(model.FirstName), "FirstName" },
                { new StringContent(model.LastName), "LastName" },
                { new StringContent(model.DOB.ToString("yyyy-MM-dd")), "DOB" },
                { new StringContent(model.Nickname), "Nickname" },
                { new StringContent(model.Email), "Email" },
                { new StringContent(model.Password), "Password" },
                { new StringContent(model.ConfirmPassword), "ConfirmPassword" },
            };

            if(model.ProfilePicture != null)
            {
                var fileStream = model.ProfilePicture.OpenReadStream();
                var fileContent = new StreamContent(fileStream);
                content.Add(fileContent, "ProfilePicture", model.ProfilePicture.Name);
            }
            

            //Create the HTTP client using the BackendChat named factory
            var httpClient = _httpClientFactory.CreateClient("BackendChat");

            try
            {
                //Execute a POST request and store the response.
                using HttpResponseMessage response = await httpClient.PostAsync("register", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RegisterResponse>();
                    if (result != null)
                    {
                        var profilePictureUrl = result.ProfilePictureUrl;
                    }
                    _logger.LogInformation("User registered succesfully!");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failded to register user. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception ocurred while trying to register user");
            }
        }
    }

}
