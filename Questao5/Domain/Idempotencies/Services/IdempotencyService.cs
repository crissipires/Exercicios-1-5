using Questao5.Domain.Idempotencies.Interfaces;

namespace Questao5.Domain.Idempotencies.Services
{
    public class IdempotencyService : IIdempotencyService
    {
        private readonly IIdempotencyRepository _idempotencyRepository;

        public IdempotencyService(IIdempotencyRepository idempotencyRepository)
        {
            _idempotencyRepository = idempotencyRepository;
        }

        public async Task<Idempotency> CheckIdempotency(string idempotencyKey)
        {
            return await _idempotencyRepository.GetByIdempotencyKey(idempotencyKey);
        }

        public async Task SaveIdempotency(string idempotencyKey, object result)
        {
            var idempotency = new Idempotency
            {
                IdempotencyKey = idempotencyKey,
                Request = "",
                Result = System.Text.Json.JsonSerializer.Serialize(result)
            };

            await _idempotencyRepository.Add(idempotency);
        }
    }
}
