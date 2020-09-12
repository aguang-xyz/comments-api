using System.Linq;
using CommentsApi.Contexts;
using CommentsApi.Entities;
using CommentsApi.Extensions;

namespace CommentsApi.Repositories
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(CommentsApiDbContext context) : base(context)
        {
        }

        public PagedEntities<Comment> GetPagedByCategory(string category, int page, int pageSize) =>
            this.Set
              .Where(comment => comment.Category == category)
              .GetPaged(page, pageSize);
    }
}
