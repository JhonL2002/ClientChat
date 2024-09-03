using System.Text.Json.Serialization;

namespace ClientChat.DTOs.Chats
{
    public class ChatDTO
    {
        [JsonPropertyName("chatId")]
        public int ChatId { get; set; }

        [JsonPropertyName("chatName")]
        public string ChatName { get; set; }

        [JsonPropertyName("chatTypeId")]
        public int ChatTypeId { get; set; }
    }
}
