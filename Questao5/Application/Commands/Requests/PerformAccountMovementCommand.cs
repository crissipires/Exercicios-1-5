using MediatR;
using Questao5.Application.Handlers.Movements.DTOs;
using Questao5.Domain.Results;

namespace Questao5.Application.Commands.Requests
{
    public class PerformAccountMovementCommand : IRequest<Result<MovementResponseDTO>>
    {
        public string AccountId { get; set; }
        public decimal Amount { get; set; }
        public char MovementType { get; set; }
        public string IdempotencyKey { get; set; }
    }
}
