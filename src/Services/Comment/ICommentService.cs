using System;
using CommentsApi.Entities;

namespace CommentsApi.Services
{
    public interface ICommentService
    {
        void Add(User user, string category, string content);

        PagedEntities<CommentResponse> GetPaged(Guid? userId, string category, int page, int pageSize, string order);

        void Like(Guid commentId, Guid userId);

        void Dislike(Guid commentId, Guid userId);
    }
}
