using System.Data;
using Dapper;
using Questao5.Domain.CurrentAccounts;
using Questao5.Domain.CurrentAccounts.Interfaces;

namespace Questao5.Repository
{
    public class CurrentAccountRepository : ICurrentAccountRepository
    {
        private readonly IDbConnection _dbConnection;

        public CurrentAccountRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<CurrentAccount> GetById(string accountId)
        {
            var sql = @" SELECT 
                            idcontacorrente AS Id, 
                            numero AS Number, 
                            nome AS HolderName, 
                            ativo AS IsActive 
                        FROM contacorrente 
                        WHERE idcontacorrente = @AccountId";

            return await _dbConnection.QueryFirstOrDefaultAsync<CurrentAccount>(sql, new { AccountId = accountId });
        }
    }
}
