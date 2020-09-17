using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CommentsApi.Contexts;
using CommentsApi.Entities;
using CommentsApi.Repositories;

namespace CommentsApi.Services
{
    public class CommentService : ICommentService
    {
        private CommentsApiDbContext _dbContext;

        private IUserRepository _userRepository;
        
        private ICommentRepository _commentRepository;

        private ILikeRepository _likeRepository;

        public CommentService(CommentsApiDbContext dbContext, IUserRepository userRepository, ICommentRepository commentRepository, ILikeRepository likeRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _likeRepository = likeRepository;
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

        public PagedEntities<CommentResponse> GetPaged(Guid? userId, string category, int page, int pageSize, string order)
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

            var commentIds = pagedComments.Entities.Select(comment => comment.UserId).Distinct();

            var users = _userRepository
                .GetByIds(commentIds)
                .ToDictionary(user => user.Id, user => user);

            var likedCommentIds = userId.HasValue ?
              _likeRepository
                  .GetByCommentIdsAndUserId(commentIds, userId.Value)
                  .Select(like => like.CommentId)
                  .Distinct() :
              new Guid[] {};

            var entities = pagedComments.Entities
                .Select(comment => new CommentResponse
                {
                    Id = comment.Id,
                    Nickname = users[comment.UserId].Nickname,
                    AvatarUrl = users[comment.UserId].AvatarUrl,
                    Content = comment.Content,
                    CountOfLikes = comment.CountOfLikes,
                    Liked = likedCommentIds.Contains(comment.Id),
                    CreatedAt = comment.CreatedAt,
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

        public void Like(Guid commentId, Guid userId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Retrive the user or throw an exception.
                var comment = _dbContext.Comments.FromSqlRaw("SELECT * FROM `Comments` WHERE `Id` = {0}", commentId).Single();
          
                // If there already exist a like record, return directly. 
                if (null != _likeRepository.GetByCommentIdAndUserId(commentId, userId))
                {
                    return;    
                }

                // Try to insert like record.
                _likeRepository.Insert(new Like {
                    UserId = userId,
                    CommentId = commentId
                });

                // Increment the count of likes.
                comment.CountOfLikes++;

                // Execute related update sql.
                _dbContext.SaveChanges();

                // Commit changes.
                transaction.Commit();
            }
        }

        public void Dislike(Guid commentId, Guid userId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Retrive the user or throw an exception.
                var comment = _dbContext.Comments.FromSqlRaw("SELECT * FROM `Comments` WHERE `Id` = {0}", commentId).Single();
          
                var like = _likeRepository.GetByCommentIdAndUserId(commentId, userId);

                // If there does not exist a like record, return directly. 
                if (null == like)
                {
                    return;    
                }

                // Try to insert like record.
                _likeRepository.Remove(like);

                // Increment the count of likes.
                comment.CountOfLikes--;

                // Execute related update sql.
                _dbContext.SaveChanges();

                // Commit changes.
                transaction.Commit();
            }
        }
    }
}
