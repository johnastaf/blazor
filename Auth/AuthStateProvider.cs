using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace TaxiBlazor.Auth
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(500);

            //var anonymous = new ClaimsIdentity();
            //return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "John Doe"),
                new Claim(ClaimTypes.Role, "Administrator")
            };
            var anonymous = new ClaimsIdentity(claims, "testAuthType");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));
        }
    }
}
