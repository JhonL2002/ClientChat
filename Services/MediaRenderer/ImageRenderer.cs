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
                builder.AddAttribute(2, "class", "spinner-border custom-spinner");
                builder.AddAttribute(3, "role", "status");

                builder.OpenElement(4, "span");
                builder.AddAttribute(5, "class", "visually-hidden");
                builder.AddContent(6, "Loading...");
                builder.CloseElement();
                builder.CloseElement();
            }
            builder.OpenElement(7, "img");
            builder.AddAttribute(8, "src", message.MediaUrl);
            builder.AddAttribute(9, "width", "100px");
            builder.AddAttribute(10, "height", "100px");
            builder.AddAttribute(11, "style", $"display:{(message.IsImageLoaded ? "block" : "none")}");
            builder.AddAttribute(12, "onload",EventCallback.Factory.Create(this, () =>
            {
                onImageLoaded.Invoke();
            }));
            builder.CloseElement();
            builder.CloseElement();
        };

    
    }


}
