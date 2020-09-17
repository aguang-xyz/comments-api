using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using CommentsApi.Entities;
using CommentsApi.Repositories;
using CommentsApi.Services;

namespace CommentsApi.Controllers
{
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private IUserService _userService;

        private ICommentService _commentService;

        public CommentsController(IUserService userService, ICommentService commentService)
        {
            _userService = userService;
            _commentService = commentService;
        }

        [HttpGet]
        [Route("comments")]
        public PagedEntities<CommentResponse> GetComments(
            [FromQuery] string category,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string order = "recent")
        {
            var user = _userService.Current;

            return _commentService.GetPaged(user?.Id,
                category, page, pageSize, order);
        }

        [HttpPost]
        [Route("comments")]
        public IActionResult AddComment(AddCommentRequest request)
        {
            var user = _userService.Current;

            if (user == null)
            {
                return Forbid();
            }

            _commentService.Add(user, request.Category, request.Content);

            return Ok();
        }

        [HttpPost]
        [Route("comments/{commentId}/likes")]
        public IActionResult Like(Guid commentId)
        {
            var user = _userService.Current;

            if (user == null)
            {
                return Forbid();
            }

            _commentService.Like(commentId, user.Id);

            return Ok();
        }

        [HttpDelete]
        [Route("comments/{commentId}/likes")]
        public IActionResult Dislike(Guid commentId)
        {
            var user = _userService.Current;

            if (user == null)
            {
                return Forbid();
            }

            _commentService.Dislike(commentId, user.Id);

            return Ok();
        }
    }
}
