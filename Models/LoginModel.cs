using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using TaxiBlazor.Auth;

namespace TaxiBlazor.Models
{
    public class LoginModel : ComponentBase
    {
        [Inject] public ILocalStorageService LocalStorageService { get; set; }
        [Inject] public NavigationManager NavManager { get; set; }
        public LoginViewModel LoginData { get; set; }
        public LoginModel()
        {
            LoginData = new LoginViewModel();
        }

        protected async Task LoginAsync()
        {
            var token = new SecurityToken
            {
                AccessToken = LoginData.Password,
                UserName = LoginData.UserName,
                ExpireAt = DateTime.UtcNow.AddDays(1)
            };

            await LocalStorageService.SetAsync("SecurityToken", token);

            NavManager.NavigateTo("/");
        }
    }

    public class LoginViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Too long")]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class SecurityToken
    {
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
