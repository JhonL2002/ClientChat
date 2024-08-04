using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ClientChat.Services
{
    public class ManageTokenService
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private const string TokenKey = "authToken";

        public ManageTokenService(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task SetToken (string token)
        {
            await _sessionStorage.SetAsync (TokenKey, token);
        }

        public async Task<string?> GetToken ()
        {
            var result = await _sessionStorage.GetAsync<string>(TokenKey);
            return result.Success ? result.Value : null;
        }

        public async Task RemoveToken()
        {
            await _sessionStorage.DeleteAsync(TokenKey);
        }
    }
}
