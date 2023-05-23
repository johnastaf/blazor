using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TaxiBlazor.Models;

namespace TaxiBlazor.Auth
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        public AuthStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorageService.GetAsync<SecurityToken>("SecurityToken");

            if (token is null || string.IsNullOrWhiteSpace(token.AccessToken) || token.ExpireAt < DateTime.Now)
            {
                var anonymous = new ClaimsIdentity();
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, token.UserName),
                new Claim(ClaimTypes.Expired, token.ExpireAt.ToLongDateString()),
                new Claim(ClaimTypes.Role, "Administrator")
            };
            var identity = new ClaimsIdentity(claims, token.AccessToken);

            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

        public async Task MakeAnonymus()
        {
            await _localStorageService.RemoveAsync("SecurityToken");

            var anonymous = new ClaimsIdentity();
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
