using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entity;

namespace Post.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostLookupController : ControllerBase
    {
        private readonly ILogger<PostLookupController> _logger;
        private readonly IQueryDispatcher<PostEntity> _dispatcher;

        public PostLookupController(ILogger<PostLookupController> logger, IQueryDispatcher<PostEntity> dispatcher)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }

        [HttpGet]
        [Route("all")]

        public async Task<IActionResult> ListAllAsync()
        {
            try
            {
                var query = new FindAllPostQuery();

                var posts = await _dispatcher.SendAsync(query);

                if (posts == null || !posts.Any())
                {
                    return NotFound();
                }
                var count = posts.Count;

                return new OkObjectResult(new PostLookupResponse()
                {
                    Message = $"{count} post found",
                    Posts = posts,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while processing request {nameof(ListAllAsync)}.");

                return StatusCode(500, "Error while processing request to retrive all posts.");
            }
        }
        [HttpGet]
        [Route("byId/{id}")]
        public async Task<IActionResult> FindPostByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id can not be empty");
            }
            try
            {
                var query = new FindPostByIdQuery() { Id = id };

                var posts = await _dispatcher.SendAsync(query);

                if (posts == null || !posts.Any())
                {
                    return NotFound();
                }

                return new OkObjectResult(new PostLookupResponse()
                {
                    Message = $"{id} post found",
                    Posts = posts,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while processing request {nameof(FindPostByIdAsync)}.");

                return StatusCode(500, "Error while processing request to retrive post by Id.");
            }
        }
        [HttpGet]
        [Route("author/{author}")]

        public async Task<IActionResult> FindPostsByAuthorAsync(string author)
        {
            if (string.IsNullOrEmpty(author))
            {
                return BadRequest("Author can not be empty");
            }

            try { 
            var query = new FindPostsByAuthorQuery() { Author = author };

            var posts = await _dispatcher.SendAsync(query);

            if (posts == null || !posts.Any())
            {
                return NotFound();
            }
            var count = posts.Count;

            return new OkObjectResult(new PostLookupResponse()
            {
                Message = $"{count} post found",
                Posts = posts,
            });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while processing request {nameof(FindPostsByAuthorAsync)}.");

                return StatusCode(500, "Error while processing request to retrive posts by author.");
            }
        }
        [HttpGet]
        [Route("withcomments")]

        public async Task<IActionResult> FindPostWithCommentsAsync()
        {
            try { 
            var query = new FindPostsWithCommentsQuery();

            var posts = await _dispatcher.SendAsync(query);

            if (posts == null || !posts.Any())
            {
                return NotFound();
            }
            var count = posts.Count;

            return new OkObjectResult(new PostLookupResponse()
            {
                Message = $"{count} post found",
                Posts = posts,
            });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while processing request {nameof(FindPostWithCommentsAsync)}.");

                return StatusCode(500, "Error while processing request to retrive posts with comments.");
            }
        }
        [HttpGet]
        [Route("likes/{numberOfLikes}")]
        public async Task<IActionResult> FindPostsWithLikesAsync(int numberOfLikes)
        {
            if(numberOfLikes < 1)
            {
                return BadRequest("Number of likes must be at least 1");
            }

            try { 
            var query = new FindPostsWithLikesQuery() { NumberOfLikes = numberOfLikes };

            var posts = await _dispatcher.SendAsync(query);

            if (posts == null || !posts.Any())
            {
                return NotFound();
            }
            var count = posts.Count;

            return new OkObjectResult(new PostLookupResponse()
            {
                Message = $"{count} post found",
                Posts = posts,
            });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while processing request {nameof(FindPostsWithLikesAsync)}.");

                return StatusCode(500, "Error while processing request to retrive posts with likes.");
            }
        }
    }
}
