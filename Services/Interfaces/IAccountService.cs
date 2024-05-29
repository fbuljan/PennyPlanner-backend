using PennyPlanner.DTOs.Account;
using PennyPlanner.Models;

namespace PennyPlanner.Services.Interfaces
{
    public interface IAccountService
    {
        Task<int> CreateAccountAsync(AccountCreate accountCreate);
        Task UpdateAccountAsync(AccountUpdate accountUpdate);
        Task DeleteAccountAsync(AccountDelete accountDelete);
        //Task AddTransactionAsync(int accountId, Transaction transaction);
        Task<AccountGet> GetAccountAsync(int id);
        Task<List<AccountGet>> GetAccountsAsync();
    }
}
