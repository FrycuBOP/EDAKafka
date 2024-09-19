using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.cmd.Api.Controllers;
using Post.Cmd.Api.Commands;
using Post.Common.DTO;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EditPostController : ControllerBase
    {
        private readonly ILogger<EditPostController> _logger;
        private readonly ICommandDispatcher _commandDispatcher;

        public EditPostController(ILogger<EditPostController> logger, ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> EditMessageAsync(Guid id,[FromBody] EditMessageCommand command)
        {
            try
            {
                command.Id = id;
                await _commandDispatcher.SendAsync(command);

                return Ok(new BaseResponse
                {
                    Message = "Edit message request completed successfully."
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
                    Message = "Error while processing request to edit message."
                }); ;
            }
        }
    }
}
