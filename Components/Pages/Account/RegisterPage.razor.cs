using ClientChat.DTOs;
using ClientChat.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace ClientChat.Components.Pages.Account
{
    public partial class RegisterPage : ComponentBase
    {
        public RegisterDTO Register = new();

        private async Task RegisterClicked()
        {
            SuccessResponse response = await registrationService.OnRegisterAsync(Register);

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
            Register.ProfilePicture = e.File;
        }
    }
}
