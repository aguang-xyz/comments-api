using System;

namespace CommentsApi.Entities
{
    public class Comment : EntityBase
    {
        // Publisher id.
        public Guid UserId { get; set; }

        // Category. 
        public string Category { get; set; }

        // Content (markdown). 
        public string Content { get; set; }

        // Publishing time. 
        public DateTime PublishTime { get; set; }

        // Count of likes.
        public int CountOfLikes { get; set; }
    }
}
