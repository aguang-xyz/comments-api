using CommentsApi.Entities;

namespace CommentsApi.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        PagedEntities<Comment> GetPagedByCategory(string category, int page, int pageSize);
    }
}
