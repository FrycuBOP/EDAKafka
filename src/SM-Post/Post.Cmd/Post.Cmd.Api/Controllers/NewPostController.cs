using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTO;

namespace Post.cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewPostController : ControllerBase
    {
        private readonly ILogger<NewPostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public NewPostController(ILogger<NewPostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewPostCommand command)
        {
            var id = Guid.NewGuid();
            try
            {
                command.Id = id;

                await _commandDispatcher.SendAsync(command);

                return new CreatedResult(string.Empty, new NewPostResponse
                {
                    Id= id,
                    Message = "New post cration request completed successfully"
                });
            }catch(InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Client made a bad request");

                return new BadRequestObjectResult(new
                {
                    ex.Message
                });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new
                {
                    Id = id,
                    Message = "Error while processing request to create new post."
                });;
            }
        }
    }
}
