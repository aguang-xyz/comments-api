using System;
using System.Linq;
using CommentsApi.Entities;

namespace CommentsApi.Extensions
{
    public static class GetPagedExtension
    {
        public static PagedEntities<TEntity> GetPaged<TEntity>(this IQueryable<TEntity> query, int page, int pageSize) where TEntity : EntityBase
        {
            var total = query.Count();
            var pageCount = (int)Math.Ceiling((double)total / pageSize);
            var entities = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedEntities<TEntity>
            {
                CurrentPage = page,
                PageCount = pageCount,
                PageSize = pageSize,
                Total = total,
                Entities = entities
            };
        }
    }
}
