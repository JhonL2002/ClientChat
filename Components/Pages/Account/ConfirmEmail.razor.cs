using ClientChat.Responses;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Text;
using ClientChat.DTOs;
using System.Linq.Expressions;

namespace ClientChat.Components.Pages.Account
{
    public partial class ConfirmEmail : ComponentBase
    {
        [Parameter] public string UserNickname { get; set; }
        [Parameter] public string Token { get; set; }

        private bool isLoading;
        private string? message;

        protected override async Task OnInitializedAsync()
        {
            var uri = new Uri(NavigationManager.Uri);

            var queryParameters = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

            UserNickname = queryParameters["userNickname"];
            Token = queryParameters["token"];

            if (string.IsNullOrEmpty(UserNickname) || string.IsNullOrEmpty(Token))
            {
                message = "Invalid email confirmation parameters";
                isLoading = false;
                return;
            }

            var httpClient = _httpClientFactory.CreateClient("BackendChat");
            var encodedToken = Uri.EscapeDataString(Token);

            //Create the json content only with "EmailConfirmed" property
            var confirmEmailDto = new ConfirmEmailDto
            {
                EmailConfirmed = true
            };

            var requestUri = $"confirm-email?userNickname={UserNickname}&token={encodedToken}";

            try
            {
                using HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUri, confirmEmailDto);
                if (response.IsSuccessStatusCode)
                {
                    message = "Your email has been confirmed";
                }
                else
                {
                    message = "Something went wrong, verify the confirmation link";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}
