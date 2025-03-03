namespace Questao5.Domain.Movements.Interfaces
{
    public interface IMovementRepository
    {
        Task Add(Movement movement);
        Task<IEnumerable<Movement>> GetByAccountId(string accountId);
    }
}
