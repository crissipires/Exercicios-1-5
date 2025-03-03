using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries;
using Questao5.Application.Queries.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrentAccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CurrentAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("perform-movement")]
        public async Task<IActionResult> PerformMovement([FromBody] PerformAccountMovementCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

        [HttpGet("balance/{accountId}")]
        public async Task<IActionResult> GetBalance(string accountId)
        {
            var query = new GetAccountBalanceQuery { AccountId = accountId };
            var result = await _mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }
    }
}
