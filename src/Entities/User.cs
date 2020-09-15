using System;

namespace CommentsApi.Entities
{
    public class User : EntityBase
    {
        // Nickname.
        public string Nickname { get; set; }

        // Email
        public string Email { get; set; }

        // Github Url.
        public string GithubUrl { get; set; }

        // Avatar Url.
        public string AvatarUrl { get; set; }
    }
}
