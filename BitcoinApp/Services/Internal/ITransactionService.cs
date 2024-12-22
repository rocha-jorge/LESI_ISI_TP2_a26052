namespace BitcoinApp.Services.Internal
{
    public interface ITransactionService
    {
        Task AddTransactionAsync(int userId, string transactionType, int units, DateTime btcTimestamp);
    }
}
