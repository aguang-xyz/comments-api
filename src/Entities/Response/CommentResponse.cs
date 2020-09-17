using System;

namespace CommentsApi.Entities
{
    public class CommentResponse
    {
        // Comment id.
        public Guid Id { get; set; }

        // Nickname.
        public string Nickname { get; set; }

        // Avatar Url.
        public string AvatarUrl { get; set; }

        // Content (markdown). 
        public string Content { get; set; }

        // Count of likes.
        public int CountOfLikes { get; set; }

        // Liked.
        public bool Liked { get; set; }

        // Created at.
        public DateTime CreatedAt { get; set; }
    }
}
