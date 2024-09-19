
using ClientChat.DTOs;
using ClientChat.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace ClientChat.Services.Chat
{
    public class ChatHubService : IAsyncDisposable
    {
        private readonly HubConnection _connection;
        private readonly ManageTokenService _manageTokenService;
        private readonly List<ChatMessageDTO> _messages = new List<ChatMessageDTO>();
        public IEnumerable<ChatMessageDTO> Messages => _messages;
        public event Action<string, string, string>? OnMessagesUpdated;

        public ChatHubService(ManageTokenService manageTokenService)
        {
            _manageTokenService = manageTokenService;
            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7210/chathub", options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        var token = await _manageTokenService.GetToken();
                        return token;
                    };
                })
                .WithAutomaticReconnect()
                .Build();

            _connection.On<string, string, string>("ReceiveMessage", (userName, message, mediaUrl) =>
            {
                _messages.Add(new ChatMessageDTO
                {
                    UserName = userName,
                    Text = message,
                    MediaUrl = mediaUrl,
                    Timestamp = DateTime.UtcNow
                });
                OnMessagesUpdated?.Invoke(userName, message, mediaUrl);
            });

            _connection.On<string>("JoinedGroup", (groupName) =>
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Joined group: {groupName}");
            });
        }

        public async Task StartAsync()
        {
            if (_connection.State == HubConnectionState.Disconnected)
            {
                _messages.Clear();
                await _connection.StartAsync();
            }
        }

        public async Task JoinGroupAsync(int chatId)
        {
            await _connection.SendAsync("JoinGroup", chatId);
        }

        public async Task SendMessageToGroupAsync(int chatId, string userName, string? message, string? mediaUrl)
        {
            await _connection.SendAsync("SendMessageToGroup", chatId, userName, message, mediaUrl);
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection.State != HubConnectionState.Disconnected)
            {
                await _connection.StopAsync();
            }
        }
    }
}
