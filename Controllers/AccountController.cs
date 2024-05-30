using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PennyPlanner.DTOs.Account;
using PennyPlanner.Services.Interfaces;

namespace PennyPlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private IAccountService AccountService { get; }

        public AccountController(IAccountService accountService)
        {
            AccountService = accountService;
        }

        //[Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount(AccountCreate accountCreate)
        {
            var id = await AccountService.CreateAccountAsync(accountCreate);
            var account = await AccountService.GetAccountAsync(id);
            var response = new { success = true, account };

            return Ok(response);
        }

        //[Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAccount(AccountUpdate accountUpdate)
        {
            await AccountService.UpdateAccountAsync(accountUpdate);
            var account = AccountService.GetAccountAsync(accountUpdate.Id);

            return Ok(account);
        }

        //[Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAccount(AccountDelete accountDelete)
        {
            await AccountService.DeleteAccountAsync(accountDelete);
            return Ok();
        }

        //[Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetAccount(int id)
        {
            var account = await AccountService.GetAccountAsync(id);
            return Ok(account);
        }

        //[Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetAccounts()
        {
            var accounts = await AccountService.GetAccountsAsync();
            return Ok(accounts);
        }
    }
}
