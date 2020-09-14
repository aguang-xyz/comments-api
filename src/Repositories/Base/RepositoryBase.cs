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
            return this.Set.Where(entity => entity.Id == id).FirstOrDefault();
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
            var entries = _context.ChangeTracker
              .Entries()
              .Where(entry => entry.Entity is EntityBase);

            entries
              .Where(entry => entry.State == EntityState.Added)
              .ToList()
              .ForEach(entry => ((EntityBase)entry.Entity).CreatedAt = DateTime.Now);

            entries
              .ToList()
              .ForEach(entry => ((EntityBase)entry.Entity).UpdatedAt = DateTime.Now);

            _context.SaveChanges();
        }
    }
}
