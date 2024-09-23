using ClientChat.DTOs;
using ClientChat.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Linq;
using System.Security.Claims;

namespace ClientChat.Components.Pages.Chat
{
    public partial class MessageComponent : ComponentBase, IAsyncDisposable
    {
        [Parameter] public int ChatId { get; set; }
        private ChatMessageDTO newMessage = new ChatMessageDTO();
        private List<ChatMediaResponse> chatMessages = new List<ChatMediaResponse>();
        private string? mediaUrl;
        private int? lastMessageId;
        private bool isLoading = false;
        private bool hasRendered = false;

        protected override async Task OnInitializedAsync()
        {
            await chatHubService.StartAsync();
            await chatHubService.JoinGroupAsync(ChatId);
            chatHubService.OnMessagesUpdated += async (userId, msg, mediaUrl) =>
            {
                await InvokeAsync(() =>
                {
                    StateHasChanged();
                });
            };
            await LoadMoreMessages(initialLoad: true);

        }


        private async Task LoadMoreMessages(bool initialLoad = false)
        {
            if (isLoading) return;

            isLoading = true;
            StateHasChanged();
            try
            {
                if (initialLoad)
                {
                    lastMessageId = null;
                }

                await chatService.GetMessagesAsync(ChatId, lastMessageId);
                if (chatService.Messages.Any())
                {
                    if (initialLoad)
                    {
                        chatMessages = chatService.Messages.OrderBy(m => m.Timestamp).ToList();
                    }
                    else
                    {
                        var orderedMessages = chatService.Messages.OrderBy(m => m.Timestamp).ToList();
                        chatMessages.InsertRange(0, orderedMessages);
                    }
                    lastMessageId = chatService.Messages.Last().MessageId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                isLoading = false;

                StateHasChanged();
            }
        }
        private void HandleSelected(InputFileChangeEventArgs e)
        {
            newMessage.File = e.File;
        }

        private async Task SendMessageToGroup()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            newMessage.ChatId = ChatId;
            newMessage.UserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            newMessage.UserName = user.FindFirst(ClaimTypes.Name).Value;
            //Send message to database
            MessageResponse response = await chatService.SendMediaAsync(newMessage);
            if (response != null)
            {
                mediaUrl = response.MediaUrl;
            }
            await chatHubService.SendMessageToGroupAsync(ChatId, newMessage.UserName, newMessage.Text, mediaUrl);
        }

        public async ValueTask DisposeAsync()
        {
            await chatHubService.DisposeAsync();
        }
    }
}
