namespace Questao5.Domain.Idempotencies.Services
{
    public interface IIdempotencyService
    {
        Task<Idempotency> CheckIdempotency(string idempotencyKey);
        Task SaveIdempotency(string idempotencyKey, object result);
    }
}
