using ClientChat.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ClientChat.Components.Pages
{
    public partial class RegisterPage : ComponentBase
    {
        public RegisterDTO Register = new();

        private async Task RegisterClicked()
        {
            await registrationService.OnRegisterAsync(Register);
        }

        private void HandleSelected(InputFileChangeEventArgs e)
        {
            Register.ProfilePicture = e.File;
        }
    }
}
