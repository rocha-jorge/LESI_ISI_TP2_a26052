namespace BitcoinApp.Models
{
    /// <summary>
    /// Representa uma transação no sistema BitcoinApp.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Identificador único da transação.
        /// </summary>
        public int idTransaction { get; set; }

        /// <summary>
        /// Identificador único do utilizador associado à transação.
        /// </summary>
        public int idUser { get; set; }

        /// <summary>
        /// Tipo de transação (por exemplo, "buy" ou "sell"). Limitado a quatro caracteres.
        /// </summary>
        public string transactionType { get; set; }

        /// <summary>
        /// Número de unidades envolvidas na transação.
        /// </summary>
        public int units { get; set; }

        /// <summary>
        /// Data e hora da transação no formato ISO 8601.
        /// Exemplo: 2024-12-22T23:39:30.700
        /// </summary>
        public DateTime btcTimeStamp { get; set; }
    }
}