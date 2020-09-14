using System.Security.Principal;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CommentsApi.Entities;
using CommentsApi.Services;

namespace CommentsApi.Controllers
{
    public class OAuthController : Controller
    {
        private IUserService _userService;

        public OAuthController(IUserService userService)
        {
            _userService = userService;
        }

        private IActionResult Login(string provider, string redirectUri) =>
      Challenge(new AuthenticationProperties { RedirectUri = redirectUri }, provider);

        [HttpGet("~/oauth/login/github")]
        public IActionResult LoginGitHub([FromQuery] string redirectUri = "/") =>
                        Login("GitHub", redirectUri);

        [HttpGet("~/oauth/whoami")]
        public User Whoami() => _userService.Current;

        [HttpGet("~/oauth/logout")]
        public IActionResult Logout() =>
                        SignOut(new AuthenticationProperties { RedirectUri = "/" },
                                        CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
