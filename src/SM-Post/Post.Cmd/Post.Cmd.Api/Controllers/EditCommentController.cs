using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTO;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EditCommentController : ControllerBase
    {
        private readonly ILogger<EditCommentController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public EditCommentController(ILogger<EditCommentController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditCommentAsync(Guid id, [FromBody] EditCommentCommand command)
        {
            try
            {
                command.Id = id;
                await _commandDispatcher.SendAsync(command);

                return Ok(new BaseResponse
                {
                    Message = "Edit command request completed successfully."
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
            catch (AggregateNotFoundException ex)
            {
                _logger.LogWarning(ex, "Could not retrive aggregate");

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
                    Id = id,
                    Message = "Error while processing request to edit command."
                }); ;
            }
        }
    }
}
