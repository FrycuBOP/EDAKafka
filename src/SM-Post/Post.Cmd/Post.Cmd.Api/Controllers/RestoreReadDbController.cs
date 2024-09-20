using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.cmd.Api.Controllers;
using Post.Cmd.Api.Commands;
using Post.Common.DTO;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RestoreReadDbController : ControllerBase
    {
        private readonly ILogger<RestoreReadDbController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public RestoreReadDbController(ILogger<RestoreReadDbController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> RestoreReadDbAsync()
        {
            try
            {
                var command = new RestoreReadDbCommand();

                await _commandDispatcher.SendAsync(command);

                return new CreatedResult(string.Empty, new BaseResponse
                {
                    Message = "Read db restore request completed successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Client made a bad request");

                return new BadRequestObjectResult(new
                {
                    ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, new
                {
                    Message = "Error while processing request to restore read database."
                }); ;
            }
        }
    }
}
