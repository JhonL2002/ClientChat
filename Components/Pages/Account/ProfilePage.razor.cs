using ClientChat.DTOs;
using ClientChat.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Microsoft.Win32;
using System.Security.Claims;

namespace ClientChat.Components.Pages.Account
{
    public partial class ProfilePage : ComponentBase
    {
        private bool isLoading = true;
        private string userName;
        private string authMessage;
        private AuthenticationState _authenticationState;
        public UpdateDTO Update = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
                if (user.Identity?.IsAuthenticated == true)
                {
                    userName = user.Identity.Name!;
                    authMessage = "User is authenticated!";
                    await LoadUserData();
                }
                else
                {
                    authMessage = "The user is not auth";
                }
                isLoading = false;
                StateHasChanged();
            }
        }

        private async Task LoadUserData()
        {
            await userDataService.GetUserDataAsync();
            var userData = userDataService.UserData;
            if (userData != null)
            {
                Update.FirstName = userData.FirstName;
                Update.LastName = userData.LastName;
                Update.Nickname = userData.Nickname;
                Update.DOB = userData.DOB;
                Update.Email = userData.Email;
                Update.ProfilePictureUrl = userData.ProfilePictureUrl;
            }
        }

        private async Task RegisterClicked()
        {
            SuccessResponse response = await updateService.OnUpdateAsync(Update);

            if (response.IsSuccess)
            {
                await js.InvokeVoidAsync("alert", "Success!");
            }
            else
            {
                await js.InvokeVoidAsync("alert", "error 400 xd");
            }
        }

        private void HandleSelected(InputFileChangeEventArgs e)
        {
            Update.ProfilePicture = e.File;
        }
    }
}
