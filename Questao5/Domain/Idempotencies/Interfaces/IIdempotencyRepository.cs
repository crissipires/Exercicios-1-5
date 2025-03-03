namespace Questao5.Domain.Idempotencies.Interfaces
{
    public interface IIdempotencyRepository
    {
        Task<Idempotency> GetByIdempotencyKey(string idempotencyKey);
        Task Add(Idempotency idempotency);
    }
}
