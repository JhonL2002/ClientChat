using ClientChat.DTOs;
using ClientChat.Responses;
using ClientChat.Services.Chat;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ClientChat.Components.Pages.Chat
{
    public partial class ChatPage : ComponentBase
    {
        public ChatMedia ChatMedia = new();

        protected override async Task OnInitializedAsync()
        {
            chatHubService.OnMessagesUpdated += UpdateUI;
            await chatHubService.StartAsync();
        }

        private void UpdateUI()
        {
            InvokeAsync(StateHasChanged);
        }

        private async Task SendMessage()
        {
            if (ChatMedia.File == null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No file found");
            }

            ChatMediaResponse res = await chatService.SendMediaAsync(ChatMedia);
            if (res.MediaUrl != null)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Message sent successfully!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No media file found!");
            }
        }

        private void HandleSelected(InputFileChangeEventArgs e)
        {
            ChatMedia.File = e.File;
        }

        private async void ClicButton()
        {
            await SendMessage();
        }

        public void Dispose()
        {
            chatHubService.OnMessagesUpdated -= UpdateUI;
        }
    }
}
