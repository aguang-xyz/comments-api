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
        private ILogger<CommentsController> _logger;

        private IUserService _userService;

        private ICommentService _commentService;

        public CommentsController(ILogger<CommentsController> logger, IUserService userService, ICommentService commentService)
        {
            _logger = logger;
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
            _logger.LogInformation($"Get comments, category = {category}, page = {page}, pageSize = {pageSize}");

            return _commentService.GetPaged(category, page, pageSize, order);
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
    }
}
