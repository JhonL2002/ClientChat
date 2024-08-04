using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClientChat.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        private readonly ManageTokenService _tokenService;

        public CustomAuthenticationStateProvider(ManageTokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _tokenService.GetToken();
                if ( string.IsNullOrEmpty(token))
                {
                    return new AuthenticationState(_anonymous);
                }

                var claimsPrincipal = CreateClaimsPrincipalFromToken(token);
                return new AuthenticationState(claimsPrincipal);
            }
            catch
            {
                return new AuthenticationState(_anonymous);
            }
        }

        public async Task UpdateAuthenticationState(string jwtToken)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            if (!string.IsNullOrEmpty(jwtToken))
            {
                await _tokenService.SetToken(jwtToken);
                claimsPrincipal = CreateClaimsPrincipalFromToken(jwtToken);
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task SetLogoutState()
        {
            await _tokenService.RemoveToken();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }

        private ClaimsPrincipal CreateClaimsPrincipalFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwtToken.Claims, "jwtAuth");

            return new ClaimsPrincipal(identity);
        }
    }
}
