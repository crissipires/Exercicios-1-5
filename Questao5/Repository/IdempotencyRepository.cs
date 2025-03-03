using Dapper;
using Questao5.Domain.Idempotencies;
using Questao5.Domain.Idempotencies.Interfaces;
using System.Data;

namespace Questao5.Repository
{
    public class IdempotencyRepository : IIdempotencyRepository
    {
        private readonly IDbConnection _dbConnection;

        public IdempotencyRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Idempotency> GetByIdempotencyKey(string idempotencyKey)
        {
            var sql = @"SELECT chave_idempotencia AS IdempotencyKey, requisicao AS Request, resultado AS Result
                         FROM idempotencia 
                        WHERE chave_idempotencia = @IdempotencyKey";

            return await _dbConnection.QueryFirstOrDefaultAsync<Idempotency>(sql, new { IdempotencyKey = idempotencyKey });
        }

        public async Task Add(Idempotency idempotency)
        {
            var sql = "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) VALUES (@IdempotencyKey, @Request, @Result)";
            await _dbConnection.ExecuteAsync(sql, idempotency);
        }
    }
}
