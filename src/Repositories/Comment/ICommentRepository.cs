using CommentsApi.Entities;

namespace CommentsApi.Repositories
{
    public interface ICommentRepository : IRepository<Comment>
    {
        PagedEntities<Comment> GetPagedByCategoryOrderByCreatedAt(
            string category, int page, int pageSize);

        PagedEntities<Comment> GetPagedByCategoryOrderByCountOfLikes(
            string category, int page, int pageSize);
    }
}
