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

        public async Task AddTransaction(int idUser, string transactionType, int units, DateTime btcTimeStamp)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var query = "INSERT INTO [Transaction] (idUser, transactionType, units, btcTimeStamp) " +
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
                Console.WriteLine($"Error occurred: {ex.Message}");
                throw;
            }
        }
        public async Task<Transaction?> GetTransactionById(int idTransaction)
        {
            Transaction? transaction = null;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var query = "SELECT * FROM [Transaction] WHERE idTransaction = @idTransaction";

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
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }

            return transaction;
        }


        public async Task<IEnumerable<Transaction>> GetTransactionsByUserId(int idUser)
        {
            var transactions = new List<Transaction>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var query = "SELECT * FROM [Transaction] WHERE idUser = @idUser";

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
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }

            return transactions;
        }

        public async Task<bool> UpdateTransaction(Transaction updatedTransaction)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var query = "UPDATE [Transaction] SET idUser = @idUser, transactionType = @transactionType, units = @units, btcTimeStamp = @btcTimeStamp WHERE idTransaction = @idTransaction";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idTransaction", updatedTransaction.idTransaction);
                        command.Parameters.AddWithValue("@idUser", updatedTransaction.idUser);
                        command.Parameters.AddWithValue("@transactionType", updatedTransaction.transactionType);
                        command.Parameters.AddWithValue("@units", updatedTransaction.units);
                        command.Parameters.AddWithValue("@btcTimeStamp", updatedTransaction.btcTimeStamp);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteTransaction(int idTransaction)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var query = "DELETE FROM [Transaction] WHERE idTransaction = @idTransaction";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idTransaction", idTransaction);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                throw;
            }
        }

    }
}
