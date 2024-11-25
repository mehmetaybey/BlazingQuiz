using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazingQuiz.Shared.Components.Auth
{
    public class QuizAuthStateProvider :AuthenticationStateProvider
    {
        private const string AuthType = "quiz-auth";
        private const string UserDataKey = "userData";

        private readonly NavigationManager _nav;
        private readonly IStorageService _storageService;
        private  Task<AuthenticationState> _authStateTask;

        public QuizAuthStateProvider(NavigationManager nav, IStorageService storageService)
        {
            _nav = nav;
            _storageService = storageService;
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
            await _storageService.SetItem(UserDataKey,user.ToJson());

        }
        public async Task SetLogoutAsync()
        {
            User = null;
            SetAuthStateTask();
            NotifyAuthenticationStateChanged(_authStateTask);
            await _storageService.RemoveItem(UserDataKey);

        }

        public bool IsInitializing { get; private set; } = true;

        public async Task InitializeAsync()
        {
            await InitializeAsync(redirectToLogin: true);
        }

        public async Task<bool> InitializeAsync(bool redirectToLogin=true)
        {
            try
            {
                var udata = await _storageService.GetItem(UserDataKey);
                if (string.IsNullOrWhiteSpace(udata))
                {
                    if(redirectToLogin)
                        RedirectToLogin();
                    return false;

                }

                var user = LoggedInUser.LoadFrom(udata);
                if (user == null || user.Id == Guid.Empty)
                {
                    //userdata is invalid
                    if(redirectToLogin)
                        RedirectToLogin();
                    return false;
                }

                if (!IsTokenValid(user.Token))
                {
                    if(redirectToLogin)
                        RedirectToLogin();
                    return false;
                }
                await SetLoginAsync(user);
                return true;

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

            return false;
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
