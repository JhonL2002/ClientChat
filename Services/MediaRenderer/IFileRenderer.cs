using ClientChat.Responses;
using Microsoft.AspNetCore.Components;

namespace ClientChat.Services.MediaRenderer
{
    public interface IFileRenderer<T>
    {
        RenderFragment RenderFile(T message, Action onImageLoaded);

    }
}
