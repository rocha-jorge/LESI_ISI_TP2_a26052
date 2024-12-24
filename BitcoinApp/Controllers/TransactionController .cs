using Microsoft.AspNetCore.Mvc;
using BitcoinApp.Models;
using BitcoinApp.Services.Internal;

namespace BitcoinApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        // New endpoint to get all transactions for a user
        [HttpGet("user/{idUser}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByUserId(int idUser)
        {
            var transactions = await _transactionService.GetTransactionsByUserIdAsync(idUser);
            if (transactions == null || !transactions.Any())
            {
                return NotFound();  // Return 404 if no transactions found
            }
            return Ok(transactions);  // Return the list of transactions
        }

        [HttpPost]
        public async Task<ActionResult> AddTransaction([FromBody] Transaction transaction)
        {
            if (transaction == null)
            {
                return BadRequest();
            }

            await _transactionService.AddTransactionAsync(transaction.idUser, transaction.transactionType, transaction.units, transaction.btcTimeStamp);
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.idTransaction }, transaction);
        }
    }
}
