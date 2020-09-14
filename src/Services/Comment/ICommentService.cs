using CommentsApi.Entities;

namespace CommentsApi.Services
{
    public interface ICommentService
    {
        void Add(User user, string category, string content);

        PagedEntities<CommentResponse> GetPaged(string category, int page, int pageSize, string order);
    }
}
