using BitcoinApp.Models;

namespace BitcoinApp.Services.Internal
{
    public interface ITransactionService
    {
        Task AddTransactionAsync(int idUser, string transactionType, int units, DateTime btcTimeStamp);
        Task<Transaction?> GetTransactionByIdAsync(int idTransaction);
        Task<IEnumerable<Transaction>> GetTransactionsByUserIdAsync(int idUser);
        Task<bool> UpdateTransactionAsync(Transaction updatedTransaction);
        Task<bool> DeleteTransactionAsync(int idTransaction);
    }
}
