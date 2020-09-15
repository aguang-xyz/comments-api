namespace CommentsApi.Entities
{
    public class CommentResponse
    {
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
    }
}
