using System;
using System.Linq;
using System.Collections.Generic;
using CommentsApi.Contexts;
using CommentsApi.Entities;

namespace CommentsApi.Repositories
{
    public class LikeRepository : RepositoryBase<Like>, ILikeRepository
    {
        public LikeRepository(CommentsApiDbContext dbContext) : base(dbContext)
        {
        }

        public Like GetByCommentIdAndUserId(Guid commentId, Guid userId) =>
            Set.Where(like => like.CommentId == commentId && like.UserId == userId)
               .FirstOrDefault();
        public IEnumerable<Like> GetByCommentIdsAndUserId(IEnumerable<Guid> commentIds, Guid userId) =>
            Set.Where(like => commentIds.Contains(like.CommentId) && like.UserId == userId)
               .ToList();
    }
}
