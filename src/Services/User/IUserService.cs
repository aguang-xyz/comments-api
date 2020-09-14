using CommentsApi.Entities;

namespace CommentsApi.Services
{
    public interface IUserService
    {
        public User Current { get; }
    }
}
