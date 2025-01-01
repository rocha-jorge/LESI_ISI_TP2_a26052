using BitcoinApp.Models;
using BitcoinApp.Services.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.ConstrainedExecution;
using System.Web.Services.Description;

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
            var transaction = await _transactionService.GetTransactionById(id);
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
            var transactions = await _transactionService.GetTransactionsByUserId(idUser);
            if (transactions == null || !transactions.Any())
            {
                return NotFound();
            }
            return Ok(transactions);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOrGuest")]
        [SwaggerOperation(
            Summary = "Registar uma nova transação. [admin, guest]",
            Description = @"O idTransaction não precisa de ser incluído no body:

            {
                ""idUser"": 1,
                ""transactionType"": ""Sell"",
                ""units"": 7,
                ""btcTimeStamp"": ""2024-12-22T23:39:30.700""
            }
            
            O timestamp tem de ser o seguinte:2024-12-22T23:39:30.700, por ser o único registo válido da entidade btcTimeStamp na BD."
        )]

        public async Task<ActionResult> AddTransaction([FromBody] Transaction transaction)
        {
            if (transaction == null)
            {
                return BadRequest();
            }

            await _transactionService.AddTransaction(transaction.idUser, transaction.transactionType, transaction.units, transaction.btcTimeStamp);
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.idTransaction }, transaction);
        }

        [HttpPut]
        [Authorize(Policy = "Admin")]
        [SwaggerOperation(
            Summary = "Atualizar as informações de uma transação. [admin]", 
            Description = @"Permite alterar as informações de uma transação específica existente na BD, por idTransaction.

            {
                ""idTransaction"": 1,
                ""idUser"": 1,
                ""transactionType"": ""Sell"",
                ""units"": 7,
                ""btcTimeStamp"": ""2024-12-22T23:39:30.700""
            }
            
            O timestamp tem de ser o seguinte:2024-12-22T23:39:30.700, por ser o único registo válido da entidade btcTimeStamp na BD."
        )]

        public async Task<IActionResult> UpdateTransaction([FromBody] Transaction updatedTransaction)
    {
            if (updatedTransaction == null || updatedTransaction.idTransaction == 0)
            {
                return BadRequest("Invalid transaction data.");
            }

            var success = await _transactionService.UpdateTransaction(updatedTransaction);

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
            var success = await _transactionService.DeleteTransaction(id);

            if (!success)
            {
                return NotFound($"Transaction with ID {id} not found.");
            }

            return NoContent();
        }
    }
}
