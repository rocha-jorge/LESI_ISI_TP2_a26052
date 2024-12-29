using BitcoinApp.Models;
using BitcoinApp.Services.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [Authorize(Policy = "AdminOrGuest")]
        [SwaggerOperation(Summary = "Obter informação de uma transação específica. [admin, guest]", Description = "Retorna as informações de uma transação específica, por idTransaction.")]
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
        [Authorize(Policy = "AdminOrGuest")]
        [SwaggerOperation(Summary = "Obter informação de todas as transações de um utilizador específico. [admin, guest]", Description = "Retorna as informações de todas as transações de um utilizador específico, por idUser.")]

        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByUserId(int idUser)
        {
            var transactions = await _transactionService.GetTransactionsByUserIdAsync(idUser);
            if (transactions == null || !transactions.Any())
            {
                return NotFound();
            }
            return Ok(transactions);
        }

        /// <remarks>
        /// O idTransaction não deve ser incluido no body:
        ///
        ///     {
        ///        "idUser": 1,
        ///        "transactionType": "Sell",
        ///        "units": 7,
        ///        "btcTimeStamp": "2024-12-22T23:39:30.700"
        ///     }
        /// O timestamp tem de ser o seguinte:
        ///
        ///     {
        ///        "btcTimeStamp": "2024-12-22T23:39:30.700"
        ///     }
        /// Por ser o único registo válido da entidade btcTimeStamp na BD.
        /// </remarks>

        [HttpPost]
        [Authorize(Policy = "AdminOrGuest")]
        [SwaggerOperation(Summary = "Registar uma nova transação. [admin, guest]", Description = "Permite escrever uma nova Transaction na BD.")]

        public async Task<ActionResult> AddTransaction([FromBody] Transaction transaction)
        {
            if (transaction == null)
            {
                return BadRequest();
            }

            await _transactionService.AddTransactionAsync(transaction.idUser, transaction.transactionType, transaction.units, transaction.btcTimeStamp);
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.idTransaction }, transaction);
        }

        /// <remarks>
        /// O timestamp tem de ser o seguinte:
        ///
        ///     {
        ///        "btcTimeStamp": "2024-12-22T23:39:30.700"
        ///     }
        /// Por ser o único registo válido da entidade btcTimeStamp na BD.
        /// </remarks>
        [HttpPut]
        [Authorize(Policy = "Admin")]
        [SwaggerOperation(Summary = "Atualizar as informações de uma transação. [admin]", Description = "Permite alterar as informações de uma transação específica existente na BD, por idTransaction.")]

        public async Task<IActionResult> UpdateTransaction([FromBody] Transaction updatedTransaction)
        {
            if (updatedTransaction == null || updatedTransaction.idTransaction == 0)
            {
                return BadRequest("Invalid transaction data.");
            }

            var success = await _transactionService.UpdateTransactionAsync(updatedTransaction);

            if (!success)
            {
                return NotFound($"Transaction with ID {updatedTransaction.idTransaction} not found.");
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        [SwaggerOperation(Summary = "Apagar uma transação. [admin]", Description = "Permite apagar uma transação específica da BD, por idTransaction.")]

        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var success = await _transactionService.DeleteTransactionAsync(id);

            if (!success)
            {
                return NotFound($"Transaction with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
