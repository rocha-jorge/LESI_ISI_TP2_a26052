using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

// For SqlConnection, SqlCommand, etc.
using Microsoft.Data.SqlClient; 


namespace BitcoinApp.Services.Internal
{
    public class TransactionService : ITransactionService
    {
        private readonly string _connectionString;

        public TransactionService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddTransactionAsync(int userId, string transactionType, int units, DateTime btcTimestamp)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO Transaction (IdUser, TransactionType, Units, BtcTimeStamp) VALUES (@UserId, @TransactionType, @Units, @BtcTimeStamp)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int) { Value = userId });
                    command.Parameters.Add(new SqlParameter("@TransactionType", SqlDbType.VarChar, 10) { Value = transactionType });
                    command.Parameters.Add(new SqlParameter("@Units", SqlDbType.Int) { Value = units });
                    command.Parameters.Add(new SqlParameter("@BtcTimeStamp", SqlDbType.DateTime) { Value = btcTimestamp });

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteTransactionAsync(int transactionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "DELETE FROM Transaction WHERE IdTransaction = @TransactionId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@TransactionId", SqlDbType.Int) { Value = transactionId });
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int transactionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT IdTransaction, IdUser, TransactionType, Units, BtcTimeStamp FROM Transaction WHERE IdTransaction = @TransactionId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@TransactionId", SqlDbType.Int) { Value = transactionId });

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Transaction
                            {
                                IdTransaction = reader.GetInt32(0),
                                IdUser = reader.GetInt32(1),
                                TransactionType = reader.GetString(2),
                                Units = reader.GetInt32(3),
                                BtcTimeStamp = reader.GetDateTime(4)
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
