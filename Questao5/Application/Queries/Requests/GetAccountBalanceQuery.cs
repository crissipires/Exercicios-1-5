using MediatR;
using Questao5.Application.Handlers.CurrentAccounts.DTOs;
using Questao5.Domain.Results;

namespace Questao5.Application.Queries.Requests
{
    public class GetAccountBalanceQuery : IRequest<Result<BalanceResponseDTO>>
    {
        public string AccountId { get; set; }
    }
}
