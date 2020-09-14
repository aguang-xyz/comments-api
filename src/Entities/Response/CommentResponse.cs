namespace CommentsApi.Entities
{
    public class CommentResponse
    {
        // Nickname.
        public string Nickname { get; set; }

        // Content (markdown). 
        public string Content { get; set; }

        // Count of likes.
        public int CountOfLikes { get; set; }
    }
}
