using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers.Movements;
using Questao5.Domain.CurrentAccounts.Interfaces;
using Questao5.Domain.CurrentAccounts;
using Questao5.Domain.Idempotencies.Services;
using Questao5.Domain.Idempotencies;
using Questao5.Domain.Movements.Interfaces;
using Questao5.Domain.Movements;
using Xunit;
using Questao5.Application.Handlers.Movements.DTOs;
using Questao5.Domain.Movements.Enumerators;

namespace Questao5.Tests.Movements
{
    public class PerformAccountMovementHandlerTests
    {
        private readonly ICurrentAccountRepository _currentAccountRepository;
        private readonly IMovementRepository _movementRepository;
        private readonly IIdempotencyService _idempotencyService;
        private readonly PerformAccountMovementHandler _handler;

        public PerformAccountMovementHandlerTests()
        {
            _currentAccountRepository = Substitute.For<ICurrentAccountRepository>();
            _movementRepository = Substitute.For<IMovementRepository>();
            _idempotencyService = Substitute.For<IIdempotencyService>();
            _handler = new PerformAccountMovementHandler(_currentAccountRepository, _movementRepository, _idempotencyService);
        }

        [Fact]
        public async Task Handle_InvalidAccount_ReturnsInvalidAccountError()
        {
            // Arrange
            var accountId = "INVALID_ACCOUNT_ID";
            var idempotencyKey = "550e8400-e29b-41d4-a716-446655440000";
            var command = new PerformAccountMovementCommand
            {
                AccountId = accountId,
                Amount = 100.00m,
                MovementType = 'C',
                IdempotencyKey = idempotencyKey
            };

            _currentAccountRepository.GetById(accountId).Returns((CurrentAccount)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

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
            var idempotencyKey = "550e8400-e29b-41d4-a716-446655440000";
            var command = new PerformAccountMovementCommand
            {
                AccountId = accountId,
                Amount = 100.00m,
                MovementType = 'C',
                IdempotencyKey = idempotencyKey
            };

            var currentAccount = new CurrentAccount
            {
                Id = accountId,
                Number = 123,
                HolderName = "Katherine Sanchez",
                IsActive = false
            };

            _currentAccountRepository.GetById(accountId).Returns(currentAccount);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("INACTIVE_ACCOUNT", result.Error.Type);
            Assert.Equal("Inactive account.", result.Error.Message);
        }

        [Fact]
        public async Task Handle_InvalidAmount_ReturnsInvalidAmountError()
        {
            // Arrange
            var accountId = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";
            var idempotencyKey = "550e8400-e29b-41d4-a716-446655440000";
            var command = new PerformAccountMovementCommand
            {
                AccountId = accountId,
                Amount = -100.00m, // Valor inválido
                MovementType = 'C',
                IdempotencyKey = idempotencyKey
            };

            var currentAccount = new CurrentAccount
            {
                Id = accountId,
                Number = 123,
                HolderName = "Katherine Sanchez",
                IsActive = true
            };

            _currentAccountRepository.GetById(accountId).Returns(currentAccount);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("INVALID_AMOUNT", result.Error.Type);
            Assert.Equal("Invalid amount.", result.Error.Message);
        }

        [Fact]
        public async Task Handle_InvalidMovementType_ReturnsInvalidMovementTypeError()
        {
            // Arrange
            var accountId = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";
            var idempotencyKey = "550e8400-e29b-41d4-a716-446655440000";
            var command = new PerformAccountMovementCommand
            {
                AccountId = accountId,
                Amount = 100.00m,
                MovementType = 'X', // Tipo inválido
                IdempotencyKey = idempotencyKey
            };

            var currentAccount = new CurrentAccount
            {
                Id = accountId,
                Number = 123,
                HolderName = "Katherine Sanchez",
                IsActive = true
            };

            _currentAccountRepository.GetById(accountId).Returns(currentAccount);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("INVALID_TYPE", result.Error.Type);
            Assert.Equal("Invalid movement type.", result.Error.Message);
        }

        [Fact]
        public async Task Handle_IdempotencyKeyExists_ReturnsPreviousResult()
        {
            // Arrange
            var accountId = "B6BAFC09-6967-ED11-A567-055DFA4A16C9";
            var idempotencyKey = "550e8400-e29b-41d4-a716-446655440000";
            var command = new PerformAccountMovementCommand
            {
                AccountId = accountId,
                Amount = 100.00m,
                MovementType = 'C',
                IdempotencyKey = idempotencyKey
            };

            var previousResult = new MovementResponseDTO { MovementId = "550e8400-e29b-41d4-a716-446655440000" };
            var idempotency = new Idempotency
            {
                IdempotencyKey = idempotencyKey,
                Result = System.Text.Json.JsonSerializer.Serialize(previousResult)
            };

            _idempotencyService.CheckIdempotency(idempotencyKey).Returns(idempotency);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(previousResult.MovementId, result.Value.MovementId);
            await _movementRepository.DidNotReceive().Add(Arg.Any<Movement>());
        }
    }
}
