using ClientChat.DTOs.Chats;
using Microsoft.AspNetCore.Components;

namespace ClientChat.Components.Pages.Chat
{
    public partial class GroupComponent : ComponentBase
    {
        private int selectedChat;
        private bool isloading = true; //Initially, the component is loading
        protected override async Task OnInitializedAsync()
        {
            isloading = true; // Initialize loading
            await userChat.GetUserChatsAsync();
            isloading = false; // When chats are loaded, disable the load
        }

        private void SelectChat(int chatId)
        {
            selectedChat = chatId;
            NavigationManager.NavigateTo($"/chat/{chatId}");
        }
    }
}
