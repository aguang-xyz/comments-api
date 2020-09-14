using System;
using System.Collections.Generic;
using CommentsApi.Entities;

namespace CommentsApi.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByEmail(string email);

        IEnumerable<User> GetByIds(IEnumerable<Guid> ids);
    }
}
