using System;
using System.Linq;
using System.Collections.Generic;
using CommentsApi.Contexts;
using CommentsApi.Entities;

namespace CommentsApi.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private CommentsApiDbContext _dbContext;

        public UserRepository(CommentsApiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public User GetByEmail(string email) =>
            Set.Where(user => user.Email == email).FirstOrDefault();

        public IEnumerable<User> GetByIds(IEnumerable<Guid> ids)
        {
            return Set.Where(user => ids.Contains(user.Id)).ToList();
        }
    }
}
