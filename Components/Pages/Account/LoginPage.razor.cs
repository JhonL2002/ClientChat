using ClientChat.DTOs;
using ClientChat.Responses;
using ClientChat.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace ClientChat.Components.Pages.Account
{
    public partial class LoginPage : ComponentBase
    {
        public LoginDTO Login = new();

        async Task LoginClicked()
        {
            LoginResponse response = await loginService.OnLoginAsync(Login);

            if (response == null)
            {
                await js.InvokeVoidAsync("alert", "Login failed");
                return;
            }

            await tokenService.SetToken(response.Token);

            var customAuthStateProvider = (CustomAuthenticationStateProvider)AuthenticationStateProvider;
            await customAuthStateProvider.UpdateAuthenticationState(response.Token);


            NavigationManager.NavigateTo("/profile", forceLoad: true);
        }
    }

}
