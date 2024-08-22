using Microsoft.AspNetCore.Components.Forms;

namespace ClientChat.DTOs
{
    public class ChatMedia
    {
        public string User { get; set; }
        public string? Text { get; set; }
        public string? MediaUrl { get; set; }
        public IBrowserFile? File { get; set; }
    }
}
