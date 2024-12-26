using BitcoinApp.Models;
using Microsoft.Data.SqlClient;
using System;

namespace BitcoinApp.Services.SOAP
{
    public class TransactionSoapService : ITransactionSoapService
    {
        private readonly string _connectionString;

        // Constructor that takes the connection string (from appsettings)
        public TransactionSoapService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Method to get the Bitcoin balance by user ID
        public decimal GetBitcoinBalanceByUserId(int idUser)
        {
            decimal balance = 0;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var query = "SELECT SUM(units) FROM [Transaction] WHERE idUser = @idUser";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idUser", idUser);

                        // Execute the query and retrieve the balance
                        var result = command.ExecuteScalar();

                        if (result != DBNull.Value)
                        {
                            balance = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                // Log the exception or handle accordingly
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                // Log the exception or handle accordingly
            }

            return balance;
        }
    }
}
