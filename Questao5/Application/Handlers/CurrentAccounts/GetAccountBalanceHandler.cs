using MediatR;
using Questao5.Application.Handlers.CurrentAccounts.DTOs;
using Questao5.Application.Handlers.Movements.DTOs;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.CurrentAccounts.Interfaces;
using Questao5.Domain.Movements.Interfaces;
using Questao5.Domain.Results;
using Questao5.Domain.Results.ErrorsMsg;

namespace Questao5.Application.Handlers.CurrentAccounts
{
    public class GetAccountBalanceHandler : IRequestHandler<GetAccountBalanceQuery, Result<BalanceResponseDTO>>
    {
        private readonly ICurrentAccountRepository _currentAccountRepository;
        private readonly IMovementRepository _movementRepository;

        public GetAccountBalanceHandler(
            ICurrentAccountRepository currentAccountRepository,
            IMovementRepository movementRepository)
        {
            _currentAccountRepository = currentAccountRepository;
            _movementRepository = movementRepository;
        }

        public async Task<Result<BalanceResponseDTO>> Handle(GetAccountBalanceQuery request, CancellationToken cancellationToken)
        {
            var currentAccount = await _currentAccountRepository.GetById(request.AccountId);
            if (currentAccount == null)
            {
                return Result.Fail<BalanceResponseDTO>(ErrorMsg.InvalidAccount);
            }

            if (currentAccount.IsActive == false)
            {
                return Result.Fail<BalanceResponseDTO>(ErrorMsg.InactiveAccount);
            }

            var movements = await _movementRepository.GetByAccountId(request.AccountId);
            var balance = movements
                .Where(m => m.MovementType == 'C').Sum(m => m.Amount) -
                movements.Where(m => m.MovementType == 'D').Sum(m => m.Amount);

            return Result.Ok(new BalanceResponseDTO
            {
                AccountNumber = currentAccount.Number,
                AccountHolderName = currentAccount.HolderName,
                InquiryDateTime = DateTime.Now,
                Balance = balance
            });
        }
    }
}
