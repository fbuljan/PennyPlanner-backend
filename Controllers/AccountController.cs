using Microsoft.AspNetCore.Mvc;
using PennyPlanner.DTOs.Account;
using PennyPlanner.Services;
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
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var id = await AccountService.CreateAccountAsync(accountCreate);
            var account = await AccountService.GetAccountAsync(id);

            var response = new { success = true, account };
            return Ok(response);
        }
    }
}
