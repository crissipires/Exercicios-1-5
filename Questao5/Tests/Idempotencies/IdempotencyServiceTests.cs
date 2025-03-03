using NSubstitute;
using Questao5.Domain.Idempotencies.Interfaces;
using Questao5.Domain.Idempotencies.Services;
using Questao5.Domain.Idempotencies;
using Xunit;
using Newtonsoft.Json;

namespace Questao5.Tests.Idempotencies
{
    public class IdempotencyServiceTests
    {
        private readonly IIdempotencyRepository _idempotencyRepository;
        private readonly IdempotencyService _service;

        public IdempotencyServiceTests()
        {
            _idempotencyRepository = Substitute.For<IIdempotencyRepository>();
            _service = new IdempotencyService(_idempotencyRepository);
        }

        [Fact]
        public async Task CheckIdempotency_KeyExists_ReturnsIdempotency()
        {
            // Arrange
            var idempotencyKey = "550e8400-e29b-41d4-a716-446655440000";
            var idempotency = new Idempotency
            {
                IdempotencyKey = idempotencyKey,
                Result = "{}"
            };

            _idempotencyRepository.GetByIdempotencyKey(idempotencyKey).Returns(idempotency);

            // Act
            var result = await _service.CheckIdempotency(idempotencyKey);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(idempotencyKey, result.IdempotencyKey);
        }

        [Fact]
        public async Task CheckIdempotency_KeyDoesNotExist_ReturnsNull()
        {
            // Arrange
            var idempotencyKey = "NON_EXISTENT_KEY";
            _idempotencyRepository.GetByIdempotencyKey(idempotencyKey).Returns((Idempotency)null);

            // Act
            var result = await _service.CheckIdempotency(idempotencyKey);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SaveIdempotency_SavesCorrectly()
        {
            // Arrange
            var idempotencyKey = "550e8400-e29b-41d4-a716-446655440000";
            var result = new { Test = "Result" };

            // Act
            await _service.SaveIdempotency(idempotencyKey, result);

            // Assert
            await _idempotencyRepository.Received(1).Add(Arg.Is<Idempotency>(x =>
                x.IdempotencyKey == idempotencyKey &&
                x.Result == JsonConvert.SerializeObject(result)
            ));
        }
    }
}
