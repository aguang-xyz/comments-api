using System;
using System.Linq;
using System.Collections.Generic;
using CommentsApi.Entities;
using CommentsApi.Repositories;

namespace CommentsApi.Services
{
    public class CommentService : ICommentService
    {
        private IUserRepository _userRepository;
        
        private ICommentRepository _commentRepository;

        public CommentService(IUserRepository userRepository, ICommentRepository commentRepository)
        {
            _userRepository = userRepository;
            _commentRepository = commentRepository;
        }

        public void Add(User user, string category, string content)
        {
            Comment comment = new Comment
            {
                UserId = user.Id,
                Category = category,
                Content = content,
                CountOfLikes = 0,
            };

            _commentRepository.Insert(comment);

            _commentRepository.SaveChanges();
        }

        public PagedEntities<CommentResponse> GetPaged(string category, int page, int pageSize, string order)
        {
            PagedEntities<Comment> pagedComments;

            switch (order)
            {
                case "recent":
                    pagedComments = _commentRepository.GetPagedByCategoryOrderByCreatedAt(category, page, pageSize);
                    break;

                case "likes":
                    pagedComments = _commentRepository.GetPagedByCategoryOrderByCountOfLikes(category, page, pageSize);
                    break;

                default:
                    throw new ArgumentException("Invalid value of order");
            }

            var users = _userRepository
              .GetByIds(pagedComments.Entities.Select(comment => comment.UserId).Distinct())
              .ToDictionary(user => user.Id, user => user);

            var entities = pagedComments.Entities
                .Select(comment => new CommentResponse
                {
                    Nickname = users[comment.UserId].Nickname,
                    Content = comment.Content,
                    CountOfLikes = comment.CountOfLikes
                })
                .ToList();

            return new PagedEntities<CommentResponse>
            {
                CurrentPage = pagedComments.CurrentPage,
                PageCount = pagedComments.PageCount,
                PageSize = pagedComments.PageSize,
                Total = pagedComments.Total,
                Entities = entities
            };
        }
    }
}
