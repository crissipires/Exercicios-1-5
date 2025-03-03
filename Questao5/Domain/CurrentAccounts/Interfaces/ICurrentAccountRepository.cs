namespace Questao5.Domain.CurrentAccounts.Interfaces
{
    public interface ICurrentAccountRepository
    {
        Task<CurrentAccount> GetById(string accountId);
    }
}
