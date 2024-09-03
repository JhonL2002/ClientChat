using System.Text.Json.Serialization;

namespace ClientChat.Responses
{
    public class GroupResponse
    {
        [JsonPropertyName("chatId")]
        public int ChatId { get; set; }

        [JsonPropertyName("chatName")]
        public string ChatName { get; set; }
    }
}
