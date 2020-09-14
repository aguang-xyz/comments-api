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

        public PagedEntities<Comment> GetPagedByCategoryOrderByCreatedAt(string category, int page, int pageSize) =>
          this.Set
            .Where(comment => comment.Category == category)
            .OrderByDescending(comment => comment.CreatedAt)
            .GetPaged(page, pageSize);

        public PagedEntities<Comment> GetPagedByCategoryOrderByCountOfLikes(string category, int page, int pageSize) =>
            this.Set
              .Where(comment => comment.Category == category)
              .OrderByDescending(comment => comment.CountOfLikes)
              .GetPaged(page, pageSize);
    }
}
