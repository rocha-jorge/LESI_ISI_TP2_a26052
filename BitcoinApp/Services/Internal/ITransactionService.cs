using BitcoinApp.Models;

namespace BitcoinApp.Services.Internal
{
    public interface ITransactionService
    {
        // Add a new transaction to the database
        Task AddTransactionAsync(int idUser, string transactionType, int units, DateTime btcTimeStamp);

        // Get all transactions for a specific user
        Task<IEnumerable<Transaction>> GetUserTransactionsAsync(int idUser);

        // Get a transaction by its ID
        Task<Transaction?> GetTransactionByIdAsync(int idTransaction);

        // Delete a transaction by ID
        Task DeleteTransactionAsync(int idTransaction);

        // Update a transaction by ID
        Task UpdateTransactionAsync(int idTransaction, string transactionType, int units, DateTime btcTimeStamp);
    }
}
