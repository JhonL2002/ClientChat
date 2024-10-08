using ClientChat.DTOs;
using ClientChat.DTOs.Chats;
using ClientChat.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace ClientChat.Components.Pages.Chat
{
    public partial class GroupPage: ComponentBase
    {
        private AuthenticationState _authenticationState;

        public GroupDTO GroupDTO = new();

        private string identifier;
        private bool isLoading = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await manageGroupService.GetAllGroupsAsync();
                isLoading = false;
                StateHasChanged();
            }
        }
        private async Task CreateGroup()
        {
            var isSuccess = await manageGroupService.CreateGroupAsync(GroupDTO);
            if (isSuccess == true)
            {
                await js.InvokeVoidAsync("alert", "Group Created Successfully");
                GroupDTO = new GroupDTO();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error to create group!");
            }
        }

        private async Task JoinToGroupButton(int chatId)
        {
            await userChat.JoinToGroupAsync(chatId);
        }
    }
}
