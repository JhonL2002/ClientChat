namespace ClientChat.Responses
{
    public class ConfirmationResult
    {
        public bool Value { get; }
        public string Message { get; }

        public ConfirmationResult(bool value, string message)
        {
            Value = value;
            Message = message;
        }
    }
}
