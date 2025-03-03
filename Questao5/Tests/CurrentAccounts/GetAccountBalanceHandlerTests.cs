using NSubstitute;
using Questao5.Application.Handlers.CurrentAccounts;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.CurrentAccounts.Interfaces;
using Questao5.Domain.CurrentAccounts;
using Questao5.Domain.Movements.Interfaces;
using Questao5.Domain.Movements;
using Xunit;

namespace Questao5.Tests.CurrentAccounts
{
    public class GetAccountBalanceHandlerTests
    {
        private readonly ICurrentAccountRepository _currentAccountRepository;
        private readonly IMovementRepository _movementRepository;
        private readonly GetAccountBalanceHandler _handler;

        public GetAccountBalanceHandlerTests()
        {
            _currentAccountRepository = Substitute.For<ICurrentAccountRepository>();
            _movementRepository = Substitute.For<IMovementRepository>();
            _handler = new GetAccountBalanceHandler(_currentAccountRepository, _movementRepository);
        }

        [Fact]
        public async Task Handle_ValidQuery_ReturnsBalanceResponse()
        {
            // Arrange
            var accountId = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";
            var query = new GetAccountBalanceQuery { AccountId = accountId };

            var currentAccount = new CurrentAccount
            {
                Id = accountId,
                Number = 123,
                HolderName = "Katherine Sanchez",
                IsActive = true
            };

            var movements = new List<Movement>
            {
                new Movement { MovementType = 'C', Amount = 100.00m },
                new Movement { MovementType = 'D', Amount = 50.00m }
            };

            _currentAccountRepository.GetById(accountId).Returns(currentAccount);
            _movementRepository.GetByAccountId(accountId).Returns(movements);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(50.00m, result.Value.Balance); // 100 (crédito) - 50 (débito) = 50
            Assert.Equal(currentAccount.Number, result.Value.AccountNumber);
            Assert.Equal(currentAccount.HolderName, result.Value.AccountHolderName);
        }

        [Fact]
        public async Task Handle_InvalidAccount_ReturnsInvalidAccountError()
        {
            // Arrange
            var accountId = "INVALID_ACCOUNT_ID";
            var query = new GetAccountBalanceQuery { AccountId = accountId };

            _currentAccountRepository.GetById(accountId).Returns((CurrentAccount)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("INVALID_ACCOUNT", result.Error.Type);
            Assert.Equal("Account not found.", result.Error.Message);
        }

        [Fact]
        public async Task Handle_InactiveAccount_ReturnsInactiveAccountError()
        {
            // Arrange
            var accountId = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";
            var query = new GetAccountBalanceQuery { AccountId = accountId };

            var currentAccount = new CurrentAccount
            {
                Id = accountId,
                Number = 123,
                HolderName = "Katherine Sanchez",
                IsActive = false
            };

            _currentAccountRepository.GetById(accountId).Returns(currentAccount);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("INACTIVE_ACCOUNT", result.Error.Type);
            Assert.Equal("Inactive account.", result.Error.Message);
        }
    }
}
