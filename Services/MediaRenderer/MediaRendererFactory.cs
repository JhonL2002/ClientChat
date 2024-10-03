using ClientChat.Bridges;
using ClientChat.Responses;

namespace ClientChat.Services.MediaRenderer
{
    public class MediaRendererFactory
    {
        public IFileRenderer<T> GetFileRenderer<T>(string extension) where T : IMediaFile
        {
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                    return new ImageRenderer<T>();
                case ".mp4":
                    //Implement renderer here!
                    return null;
                case ".mp3":
                case ".wav":
                    //Implement renderer here!
                    return null;
                case ".pdf":
                case ".docx":
                case ".xlsx":
                case ".zip":
                    return new DocumentRenderer<T>();
                default:
                    return null;
            }
        }
    }
}
