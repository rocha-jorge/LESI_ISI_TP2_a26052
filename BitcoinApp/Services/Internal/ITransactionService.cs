using BitcoinApp.Models;

namespace BitcoinApp.Services.Internal
{
    public interface ITransactionService
    {
        // Add a new transaction to the database
        Task AddTransactionAsync(int userId, string transactionType, int units, DateTime btcTimestamp);

        // Get all transactions for a specific user
        Task<IEnumerable<Transaction>> GetUserTransactionsAsync(int userId);

        // Get a transaction by ID
        Task<Transaction?> GetTransactionByIdAsync(int transactionId);

        // Delete a transaction by ID
        Task DeleteTransactionAsync(int transactionId);

        // Update a transaction by ID
        Task UpdateTransactionAsync(int transactionId, string transactionType, int units, DateTime btcTimestamp);
    }
}
