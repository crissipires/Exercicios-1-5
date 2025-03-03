using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers.Movements.DTOs;
using Questao5.Domain.CurrentAccounts;
using Questao5.Domain.CurrentAccounts.Interfaces;
using Questao5.Domain.Idempotencies.Services;
using Questao5.Domain.Movements;
using Questao5.Domain.Movements.Enumerators;
using Questao5.Domain.Movements.Interfaces;
using Questao5.Domain.Results;
using Questao5.Domain.Results.ErrorsMsg;

namespace Questao5.Application.Handlers.Movements
{
    public class PerformAccountMovementHandler : IRequestHandler<PerformAccountMovementCommand, Result<MovementResponseDTO>>
    {
        private readonly ICurrentAccountRepository _currentAccountRepository;
        private readonly IMovementRepository _movementRepository;
        private readonly IIdempotencyService _idempotencyService;

        public PerformAccountMovementHandler(
            ICurrentAccountRepository currentAccountRepository,
            IMovementRepository movementRepository,
            IIdempotencyService idempotencyService)
        {
            _currentAccountRepository = currentAccountRepository;
            _movementRepository = movementRepository;
            _idempotencyService = idempotencyService;
        }

        public async Task<Result<MovementResponseDTO>> Handle(PerformAccountMovementCommand request, CancellationToken cancellationToken)
        {
            var idempotency = await _idempotencyService.CheckIdempotency(request.IdempotencyKey);
            if (idempotency != null)
            {
                var previousResult = JsonConvert.DeserializeObject<MovementResponseDTO>(idempotency.Result);
                return Result.Ok(previousResult);
            }

            var currentAccount = await _currentAccountRepository.GetById(request.AccountId);

            var validation = ValidateMovement(request, currentAccount);
            if (validation.IsFailure)
            {
                return validation;
            }

            var movement = new Movement
            {
                Id = Guid.NewGuid().ToString(),
                AccountId = request.AccountId,
                MovementDate = DateTime.Now.ToString("dd/MM/yyyy"),
                MovementType = request.MovementType,
                Amount = request.Amount
            };

            await _movementRepository.Add(movement);

            await _idempotencyService.SaveIdempotency(request.IdempotencyKey, movement);

            return Result.Ok(new MovementResponseDTO { MovementId = movement.Id });
        }

        private static Result<MovementResponseDTO> ValidateMovement(PerformAccountMovementCommand request, CurrentAccount currentAccount)
        {
            if (currentAccount == null)
            {
                return Result.Fail<MovementResponseDTO>(ErrorMsg.InvalidAccount); 
            }

            if (!currentAccount.IsActive)
            {
                return Result.Fail<MovementResponseDTO>(ErrorMsg.InactiveAccount); 
            }

            if (request.Amount <= 0)
            {
                return Result.Fail<MovementResponseDTO>(ErrorMsg.InvalidAmount); 
            }

            if (request.MovementType != EnumMovementType.Credito && request.MovementType != EnumMovementType.Debito)
            {
                return Result.Fail<MovementResponseDTO>(ErrorMsg.InvalidMovementType);
            }


            return Result.Ok<MovementResponseDTO>();
        }
    }
}
