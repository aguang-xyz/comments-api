using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using static AspNet.Security.OAuth.GitHub.GitHubAuthenticationConstants;
using CommentsApi.Entities;
using CommentsApi.Repositories;
using CommentsApi.Extensions;

namespace CommentsApi.Services
{
    public class UserService : IUserService
    {
        private IHttpContextAccessor _httpContextAccessor;

        private IUserRepository _userRepository;

        public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public User Current
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;

                var authType = httpContext.User.Identity.AuthenticationType;


                switch (authType)
                {
                    case "GitHub":

                        // Retrieve name.
                        var name = httpContext.User.Claims
                          .Where(claim => claim.Type == ClaimTypes.Name)
                          .Select(claim => claim.Value)
                          .First();

                        // Retrieve email.
                        var email = httpContext.User.Claims
                          .Where(claim => claim.Type == ClaimTypes.Email)
                          .Select(claim => claim.Value)
                          .First();

                        // Retrieve Github url. 
                        var githubUrl = httpContext.User.Claims
                          .Where(claim => claim.Type == Claims.Url)
                          .Select(claim => claim.Value)
                          .First();

                        // Retrieve Gravatar url.
                        var gravatarUrl = $"https://github.com/{name}.png";

                        var user = _userRepository.GetByEmail(email);

                        if (null == user)
                        {
                            user = new User
                            {
                                Nickname = name,
                                Email = email,
                                GithubUrl = githubUrl,
                                GravatarUrl = gravatarUrl
                            };

                            _userRepository.Insert(user);

                            _userRepository.SaveChanges();
                        }

                        return user;

                    default:

                        return null;
                }
            }
        }
    }
}
