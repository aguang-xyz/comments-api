using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using CommentsApi.Entities;
using CommentsApi.Repositories;

namespace CommentsApi.Controllers
{
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private ILogger<CommentsController> _logger;

        private ICommentRepository _commentRepository;

        public CommentsController(ILogger<CommentsController> logger, ICommentRepository commentRepository)
        {
            _logger = logger;
            _commentRepository = commentRepository;
        }

        [HttpGet]
        [Route("comments/{category}")]
        public PagedEntities<Comment> GetComments(
            string category,
            [FromQuery(Name = "page")] int? page,
            [FromQuery(Name = "pageSize")] int? pageSize)
        {
            _logger.LogInformation($"Get comments, category = {category}, page = {page}, pageSize = {pageSize}");

            return _commentRepository.GetPagedByCategory(category, page ?? 1, pageSize ?? 10);
        }
    }
}
