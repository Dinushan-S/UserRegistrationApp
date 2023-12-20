using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace UserRegistrationApp.Data
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private bool _isInitialized;

        public CustomAuthStateProvider(ILocalStorageService localStorage, IAuthenticationService authenticationService, IUserService userService)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var state = new AuthenticationState(new ClaimsPrincipal());
            if (_isInitialized)
            {
                string user = await _localStorage.GetItemAsStringAsync("email");
                if (!string.IsNullOrEmpty(user))
                {
                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Email, user),
                    }, "auth");
                    state = new AuthenticationState(new ClaimsPrincipal(identity));
                }
            }
            return state;
        }
        public async Task InitializeAsync()
        {
            _isInitialized = true;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}