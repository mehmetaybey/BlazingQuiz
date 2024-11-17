using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using BlazingQuiz.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazingQuiz.Web.Auth
{
    public class QuizAuthStateProvider :AuthenticationStateProvider
    {
        private const string AuthType = "quiz-auth";
        private const string UserDataKey = "userData";

        private readonly IJSRuntime _jsRuntime;
        private readonly NavigationManager _nav;
        private  Task<AuthenticationState> _authStateTask;

        public QuizAuthStateProvider(IJSRuntime jsRuntime, NavigationManager nav)
        {
            _jsRuntime = jsRuntime;
            _nav = nav;
            SetAuthStateTask();
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() => _authStateTask;

        public LoggedInUser User { get; private set; }
        public bool IsLoggedIn => User != null && User.Id != Guid.Empty;

        public async Task SetLoginAsync(LoggedInUser user)
        {
            User = user;
            SetAuthStateTask();
            NotifyAuthenticationStateChanged(_authStateTask);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UserDataKey, user.ToJson());

        }
        public async Task SetLogoutAsync()
        {
            User = null;
            SetAuthStateTask();
            NotifyAuthenticationStateChanged(_authStateTask);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", UserDataKey);

        }

        public bool IsInitializing { get; private set; } = true;

        public async Task InitializeAsync()
        {
            try
            {
                var udata = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", UserDataKey);
                if (string.IsNullOrWhiteSpace(udata))
                {
                    return;

                }

                var user = LoggedInUser.LoadFrom(udata);
                if (user == null || user.Id == Guid.Empty)
                {
                    //userdata is invalid
                    RedirectToLogin();
                    return;
                }

                if (!IsTokenValid(user.Token))
                {
                    RedirectToLogin();
                    return;
                }
                await SetLoginAsync(user);

            }
            catch(Exception ex)
            {
                //TODO: fix this error 
                //SetloginAsync from this this InitializeAsync methods throws
                //Collection was modified-Enumaration has changed on the NotifyAuthenticationStateChanged  
            }
            finally
            {
                IsInitializing = false;
            } 
        }

        private void RedirectToLogin()
        {
            _nav.NavigateTo("auth/login");
        }

        private static bool IsTokenValid(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;
            var jwtHandler = new JwtSecurityTokenHandler();
            
            if (!jwtHandler.CanReadToken(token))//invalid
                return false;

            var jwt = jwtHandler.ReadJwtToken(token);
            var expClaim= jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
            if (expClaim == null)
                return false;

            var expTime = long.Parse(expClaim.Value);

            var expDatetime= DateTimeOffset.FromUnixTimeSeconds(expTime).UtcDateTime;
            
            
            return expDatetime>DateTime.UtcNow;
        }

        private void SetAuthStateTask()
        {
            if (IsLoggedIn)
            {
                var identity = new ClaimsIdentity(User.ToClaims(),AuthType);
                var principal = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(principal);
                _authStateTask = Task.FromResult(state);

            }
            else
            {
                var identity = new ClaimsIdentity();
                var principal = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(principal);
                _authStateTask = Task.FromResult(state);
            }
        }
    }
}
