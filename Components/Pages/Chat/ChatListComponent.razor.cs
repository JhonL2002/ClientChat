using ClientChat.DTOs.Chats;
using Microsoft.AspNetCore.Components;

namespace ClientChat.Components.Pages.Chat
{
    public partial class ChatListComponent : ComponentBase
    {
        private int selectedChat;
        protected override async Task OnInitializedAsync()
        {
            await userChat.GetUserChatsAsync();
        }

        private void SelectChat(int chatId)
        {
            selectedChat = chatId;
            NavigationManager.NavigateTo($"/chat/{chatId}");
        }
    }
}
