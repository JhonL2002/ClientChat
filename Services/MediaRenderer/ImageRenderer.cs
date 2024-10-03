using ClientChat.Bridges;
using ClientChat.Responses;
using Microsoft.AspNetCore.Components;

namespace ClientChat.Services.MediaRenderer
{
    public class ImageRenderer<T> : IFileRenderer<T> where T : IMediaFile
    {
        public RenderFragment RenderFile(T message, Action onImageLoaded) => builder =>
        {
            builder.OpenElement(0, "div");
            if (!message.IsImageLoaded)
            {
                builder.OpenElement(1, "div");
                builder.AddContent(2, "Loading...");
                builder.CloseElement();
            }
            builder.OpenElement(3, "img");
            builder.AddAttribute(4, "src", message.MediaUrl);
            builder.AddAttribute(5, "width", "100px");
            builder.AddAttribute(6, "height", "100px");
            builder.AddAttribute(7, "style", $"display:{(message.IsImageLoaded ? "block" : "none")}");
            builder.AddAttribute(8, "onload",EventCallback.Factory.Create(this, () =>
            {
                onImageLoaded.Invoke();
            }));
            builder.CloseElement();
            builder.CloseElement();
        };

    
    }


}
