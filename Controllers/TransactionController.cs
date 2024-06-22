using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PennyPlanner.DTOs.Transactions;
using PennyPlanner.Enums;
using PennyPlanner.Services.Interfaces;

namespace PennyPlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private ITransactionService TransactionService { get; }

        public TransactionController(ITransactionService transactionService)
        {
            TransactionService = transactionService;
        }

        //[Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateTransaction(TransactionCreate transactionCreate)
        {
            var id = await TransactionService.CreateTransactionAsync(transactionCreate, true);
            var transaction = await TransactionService.GetTransactionAsync(id);
            var response = new { success = true, transaction };

            return Ok(response);
        }

        //[Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTransaction(TransactionUpdate transactionUpdate)
        {
            await TransactionService.UpdateTransactionAsync(transactionUpdate);
            var transaction = await TransactionService.GetTransactionAsync(transactionUpdate.Id);

            return Ok(transaction);
        }

        //[Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteTransaction(TransactionDelete transactionDelete)
        {
            await TransactionService.DeleteTransactionAsync(transactionDelete);
            return Ok();
        }

        //[Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var transaction = await TransactionService.GetTransactionAsync(id);
            return Ok(transaction);
        }

        //[Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await TransactionService.GetTransactionsAsync();
            return Ok(transactions);
        }

        //[Authorize]
        [HttpGet("categories")]
        public IActionResult GetTransactionCategories()
        {
            var categories = Enum.GetValues(typeof(TransactionCategory))
                               .Cast<TransactionCategory>()
                               .Select(e => new { Name = e.ToString(), Value = (int)e })
                               .ToList();
            return Ok(categories);
        }
    }
}
