using Microsoft.AspNetCore.Mvc;
using BitcoinApp.Services.Internal;
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
