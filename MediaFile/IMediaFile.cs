namespace ClientChat.Bridges
{
    public interface IMediaFile
    {
        string? MediaUrl { get; set; }
        bool IsImageLoaded { get; set; }
    }
}
