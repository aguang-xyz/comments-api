using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using CommentsApi.Contexts;
using CommentsApi.Entities;

namespace CommentsApi.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        protected CommentsApiDbContext _context;

        public RepositoryBase(CommentsApiDbContext context)
        {
            _context = context;
        }

        protected DbSet<TEntity> Set
        {
            get
            {
                return _context.Set<TEntity>();
            }
        }

        public TEntity GetById(Guid id)
        {
            return this.Set.Single(entity => entity.Id == id);
        }

        public void Insert(TEntity entity)
        {
            this.Set.Add(entity);
        }

        public void Remove(TEntity entity)
        {
            this.Set.Remove(entity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
