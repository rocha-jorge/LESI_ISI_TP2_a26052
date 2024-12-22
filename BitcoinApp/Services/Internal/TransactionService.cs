using BitcoinApp.Models;
using Microsoft.Data.SqlClient;

namespace BitcoinApp.Services.Internal
{
    public class TransactionService : ITransactionService
    {
        private readonly string _connectionString;

        // Constructor that takes the connection string
        public TransactionService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Add a new transaction to the database
        public async Task AddTransactionAsync(int userId, string transactionType, int units, DateTime btcTimestamp)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var query = "INSERT INTO Transactions (IdUser, TransactionType, Units, BtcTimeStamp) " +
                                "VALUES (@IdUser, @TransactionType, @Units, @BtcTimeStamp)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdUser", userId);
                        command.Parameters.AddWithValue("@TransactionType", transactionType);
                        command.Parameters.AddWithValue("@Units", units);
                        command.Parameters.AddWithValue("@BtcTimeStamp", btcTimestamp);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the exception or handle it accordingly
                Console.WriteLine($"Error occurred: {ex.Message}");
                throw;  // Rethrow the exception or handle as needed
            }
        }

        // Get all transactions for a specific user
        public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync(int userId)
        {
            var transactions = new List<Transaction>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Transactions WHERE IdUser = @IdUser";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdUser", userId);

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

        // Get a transaction by its ID
        public async Task<Transaction?> GetTransactionByIdAsync(int transactionId)
        {
            Transaction? transaction = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Transactions WHERE IdTransaction = @IdTransaction";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdTransaction", transactionId);

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

            return transaction;
        }

        // Delete a transaction by ID
        public async Task DeleteTransactionAsync(int transactionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "DELETE FROM Transactions WHERE IdTransaction = @IdTransaction";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdTransaction", transactionId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Update a transaction by ID
        public async Task UpdateTransactionAsync(int transactionId, string transactionType, int units, DateTime btcTimestamp)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "UPDATE Transactions SET TransactionType = @TransactionType, Units = @Units, BtcTimeStamp = @BtcTimeStamp " +
                            "WHERE IdTransaction = @IdTransaction";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdTransaction", transactionId);
                    command.Parameters.AddWithValue("@TransactionType", transactionType);
                    command.Parameters.AddWithValue("@Units", units);
                    command.Parameters.AddWithValue("@BtcTimeStamp", btcTimestamp);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
