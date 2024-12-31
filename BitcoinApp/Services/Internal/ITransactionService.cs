using BitcoinApp.Models;

namespace BitcoinApp.Services.Internal
{
    public interface ITransactionService
    {
        Task AddTransaction(int idUser, string transactionType, int units, DateTime btcTimeStamp);
        Task<Transaction?> GetTransactionById(int idTransaction);
        Task<IEnumerable<Transaction>> GetTransactionsByUserId(int idUser);
        Task<bool> UpdateTransaction(Transaction updatedTransaction);
        Task<bool> DeleteTransaction(int idTransaction);
    }
}
