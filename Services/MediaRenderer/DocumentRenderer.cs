using ClientChat.Bridges;
using ClientChat.Responses;
using Microsoft.AspNetCore.Components;

namespace ClientChat.Services.MediaRenderer
{
    public class DocumentRenderer<T> : IFileRenderer<T> where T :IMediaFile
    {
        public RenderFragment RenderFile(T message, Action onImageLoaded) => builder =>
        {
            builder.OpenElement(0, "div");
            builder.OpenElement(1, "a");
            builder.AddAttribute(2, "href", message.MediaUrl);
            builder.AddAttribute(3, "target", "_blank");
            builder.AddContent(4, message.MediaUrl);
            builder.CloseElement();
            builder.CloseElement();
        };
    }
}
