using System;
using System.Collections.Generic;
using CommentsApi.Entities;

namespace CommentsApi.Repositories
{
    public interface ILikeRepository : IRepository<Like>
    {
        Like GetByCommentIdAndUserId(Guid commentId, Guid userId);

        IEnumerable<Like> GetByCommentIdsAndUserId(IEnumerable<Guid> commentIds, Guid userId);
    }
}
