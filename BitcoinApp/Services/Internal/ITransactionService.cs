using BitcoinApp.Models;

namespace BitcoinApp.Services.Internal
{
    public interface ITransactionService
    {
        // Add a new transaction to the database
        Task AddTransactionAsync(int idUser, string transactionType, int units, DateTime btcTimeStamp);

        // Get a transaction by its ID
        Task<Transaction?> GetTransactionByIdAsync(int idTransaction);
    }
}
