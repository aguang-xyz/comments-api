using System;

namespace CommentsApi.Entities
{
    public class Like : EntityBase
    {
        public Guid UserId { get; set; }

        public Guid CommentId { get; set; }
    }
}
