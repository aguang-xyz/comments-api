using System;
using System.Linq;
using System.Collections.Generic;

namespace CommentsApi.Entities
{
    public class PagedEntities<TEntity> where TEntity : EntityBase
    {
        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }

        public IList<TEntity> Entities { get; set; }
    }
}
