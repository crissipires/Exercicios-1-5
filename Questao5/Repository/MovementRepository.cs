using System.Data;
using Dapper;
using Questao5.Domain.Movements;
using Questao5.Domain.Movements.Interfaces;

namespace Questao5.Repository
{
    public class MovementRepository : IMovementRepository
    {
        private readonly IDbConnection _dbConnection;

        public MovementRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task Add(Movement movement)
        {
            var sql = "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES (@Id, @AccountId, @MovementDate, @MovementType, @Amount)";
            await _dbConnection.ExecuteAsync(sql, movement);
        }

        public async Task<IEnumerable<Movement>> GetByAccountId(string accountId)
        {

            var sql = @" SELECT 
                            idmovimento AS Id, 
                            idcontacorrente AS AccountId, 
                            datamovimento AS MovementDate, 
                            tipomovimento AS MovementType, 
                            valor AS Amount 
                        FROM movimento 
                        WHERE idcontacorrente = @AccountId";

            return await _dbConnection.QueryAsync<Movement>(sql, new { AccountId = accountId });
        }
    }
}
