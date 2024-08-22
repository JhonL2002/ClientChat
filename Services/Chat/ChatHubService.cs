
using ClientChat.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace ClientChat.Services.Chat
{
    public class ChatHubService : IAsyncDisposable
    {
        private readonly HubConnection _connection;
        private readonly List<ChatMedia> _messages = new List<ChatMedia>();

        public IEnumerable<ChatMedia> Messages => _messages;
        public event Action OnMessagesUpdated;

        public ChatHubService()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7210/chathub")
                .Build();

            _connection.On<string, string>("ReceiveMedia", (user, mediaUrl) =>
            {
                var newMessage = new ChatMedia
                {
                    User = user,
                    MediaUrl = mediaUrl,
                };
                _messages.Add(newMessage);
                OnMessagesUpdated?.Invoke();
            });
        }

        public async Task StartAsync()
        {
            await _connection.StartAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _connection.DisposeAsync();
        }
    }
}
