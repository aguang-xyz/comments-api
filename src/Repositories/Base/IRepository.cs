using System;
using CommentsApi.Entities;

namespace CommentsApi.Repositories
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        TEntity GetById(Guid id);

        void Insert(TEntity entity);

        void Remove(TEntity entity);

        void SaveChanges();
    }
}
