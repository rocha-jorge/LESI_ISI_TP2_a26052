namespace BitcoinApp.Models
{
    /// <summary>
    /// Represents a transaction in the BitcoinApp system.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Unique identifier for the transaction.
        /// </summary>
        public int idTransaction { get; set; }

        /// <summary>
        /// Unique identifier for the user associated with the transaction.
        /// </summary>
        public int idUser { get; set; }

        /// <summary>
        /// Type of transaction (e.g., "Buy" or "Sell").
        /// </summary>
        public string transactionType { get; set; }

        /// <summary>
        /// Number of units involved in the transaction.
        /// </summary>
        public int units { get; set; }


        /// <summary>
        /// Timestamp for the transaction in ISO 8601 format.
        /// Example: 2024-12-22T23:39:30.700
        /// </summary>
        /// 
        public DateTime btcTimeStamp { get; set; }

    }
}
