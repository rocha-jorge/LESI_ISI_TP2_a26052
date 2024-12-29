using BitcoinApp.Models;
using BitcoinApp.Services.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpGet("user/{idUser}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByUserId(int idUser)
        {
            Console.WriteLine($"Received GET request for user with idUser = {idUser}");
            var transactions = await _transactionService.GetTransactionsByUserIdAsync(idUser);
            if (transactions == null || !transactions.Any())
            {
                Console.WriteLine($"No transactions found for idUser = {idUser}");
                return NotFound();
            }
            Console.WriteLine($"Found {transactions.Count()} transactions for idUser = {idUser}");
            return Ok(transactions);
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     POST
        ///     {
        ///        "idUser": 1,
        ///        "transactionType": "Sell",
        ///        "units": 7,
        ///        "btcTimeStamp": "2024-12-22T23:39:30.700"
        ///     }
        /// </remarks>

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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] Transaction updatedTransaction)
        {
            if (updatedTransaction == null || updatedTransaction.idTransaction != id)
            {
                return BadRequest("Invalid transaction data.");
            }

            var success = await _transactionService.UpdateTransactionAsync(updatedTransaction);

            if (!success)
            {
                return NotFound($"Transaction with ID {id} not found.");
            }

            return NoContent(); // 204 No Content
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            if (id == null)
            {
                return BadRequest("Invalid transaction data.");
            }

            var success = await _transactionService.DeleteTransactionAsync(id);

            if (!success)
            {
                return NotFound($"Transaction with ID {id} not found.");
            }

            return NoContent(); // 204 No Content
        }
    }



}
