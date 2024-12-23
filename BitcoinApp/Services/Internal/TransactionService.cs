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
        public async Task AddTransactionAsync(int idUser, string transactionType, int units, DateTime btcTimeStamp)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var query = "INSERT INTO Transaction (idUser, transactionType, units, btcTimeStamp) " +
                                "VALUES (@idUser, @transactionType, @units, @btcTimeStamp)";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idUser", idUser);
                        command.Parameters.AddWithValue("@transactionType", transactionType);
                        command.Parameters.AddWithValue("@units", units);
                        command.Parameters.AddWithValue("@btcTimeStamp", btcTimeStamp);

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
        public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync(int idUser)
        {
            var transactions = new List<Transaction>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Transaction WHERE idUser = @idUser";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idUser", idUser);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            transactions.Add(new Transaction
                            {
                                idTransaction = reader.GetInt32(0),
                                idUser = reader.GetInt32(1),
                                transactionType = reader.GetString(2),
                                units = reader.GetInt32(3),
                                btcTimeStamp = reader.GetDateTime(4)
                            });
                        }
                    }
                }
            }

            return transactions;
        }

        // Get a transaction by its ID
        public async Task<Transaction?> GetTransactionByIdAsync(int idTransaction)
        {
            Transaction? transaction = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Transaction WHERE idTransaction = @idTransaction";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idTransaction", idTransaction);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            transaction = new Transaction
                            {
                                idTransaction = reader.GetInt32(0),
                                idUser = reader.GetInt32(1),
                                transactionType = reader.GetString(2),
                                units = reader.GetInt32(3),
                                btcTimeStamp = reader.GetDateTime(4)
                            };
                        }
                    }
                }
            }

            return transaction;
        }

        // Delete a transaction by ID
        public async Task DeleteTransactionAsync(int idTransaction)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "DELETE FROM Transaction WHERE idTransaction = @idTransaction";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idTransaction", idTransaction);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Update a transaction by ID
        public async Task UpdateTransactionAsync(int idTransaction, string transactionType, int units, DateTime btcTimeStamp)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "UPDATE Transaction SET transactionType = @transactionType, Units = @units, btcTimeStamp = @btcTimeStamp " +
                            "WHERE idTransaction = @idTransaction";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idTransaction", idTransaction);
                    command.Parameters.AddWithValue("@transactionType", transactionType);
                    command.Parameters.AddWithValue("@units", units);
                    command.Parameters.AddWithValue("@btcTimeStamp", btcTimeStamp);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
