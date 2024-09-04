using ClientChat.DTOs;
using ClientChat.DTOs.Chats;
using ClientChat.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace ClientChat.Components.Pages.Chat
{
    public partial class GroupComponent: ComponentBase
    {
        private AuthenticationState _authenticationState;

        public GroupDTO GroupDTO = new();

        private string identifier;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = _authenticationState.User;
                if (user.Identity is not null && user.Identity.IsAuthenticated)
                {
                    identifier = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
                    GroupDTO.CreatorUserId = Convert.ToInt32(identifier);
                }

                StateHasChanged();
            }

            await manageGroupService.GetAllGroupsAsync();
            StateHasChanged();
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
