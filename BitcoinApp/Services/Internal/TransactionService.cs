using System.Data;
using Microsoft.Data.SqlClient;
using BitcoinApp.Models;

namespace BitcoinApp.Services.Internal
{
    public class TransactionService : ITransactionService
    {
        private readonly string _connectionString;

        public TransactionService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Add a new transaction
        public async Task AddTransactionAsync(int userId, string transactionType, int units, DateTime btcTimestamp)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO Transaction (IdUser, TransactionType, Units, BtcTimeStamp) VALUES (@IdUser, @TransactionType, @Units, @BtcTimeStamp)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@IdUser", SqlDbType.Int) { Value = userId });
                    command.Parameters.Add(new SqlParameter("@TransactionType", SqlDbType.VarChar, 4) { Value = transactionType });
                    command.Parameters.Add(new SqlParameter("@Units", SqlDbType.Int) { Value = units });
                    command.Parameters.Add(new SqlParameter("@BtcTimeStamp", SqlDbType.DateTime) { Value = btcTimestamp });

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Get transactions for a user
        public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync(int userId)
        {
            var transactions = new List<Transaction>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM Transaction WHERE IdUser = @IdUser";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@IdUser", SqlDbType.Int) { Value = userId });

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            transactions.Add(new Transaction
                            {
                                IdTransaction = reader.GetInt32(0),
                                IdUser = reader.GetInt32(1),
                                TransactionType = reader.GetString(2),
                                Units = reader.GetInt32(3),
                                BtcTimeStamp = reader.GetDateTime(4)
                            });
                        }
                    }
                }
            }

            return transactions;
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int transactionId)
        {
            Transaction? transaction = null;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = "SELECT IdTransaction, IdUser, TransactionType, Units, BtcTimeStamp FROM Transaction WHERE IdTransaction = @IdTransaction";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@IdTransaction", SqlDbType.Int) { Value = transactionId });

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                transaction = new Transaction
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
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException("Error while retrieving transaction", ex);
            }

            return transaction;
        }

        public async Task DeleteTransactionAsync(int transactionId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = "DELETE FROM Transaction WHERE IdTransaction = @IdTransaction";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@IdTransaction", SqlDbType.Int) { Value = transactionId });

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException("Error while deleting transaction", ex);
            }
        }

        public async Task UpdateTransactionAsync(int transactionId, string transactionType, int units, DateTime btcTimestamp)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = "UPDATE Transaction SET TransactionType = @TransactionType, Units = @Units, BtcTimeStamp = @BtcTimeStamp WHERE IdTransaction = @IdTransaction";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@IdTransaction", SqlDbType.Int) { Value = transactionId });
                        command.Parameters.Add(new SqlParameter("@TransactionType", SqlDbType.VarChar, 4) { Value = transactionType });
                        command.Parameters.Add(new SqlParameter("@Units", SqlDbType.Int) { Value = units });
                        command.Parameters.Add(new SqlParameter("@BtcTimeStamp", SqlDbType.DateTime) { Value = btcTimestamp });

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException("Error while updating transaction", ex);
            }
        }

    }
}
