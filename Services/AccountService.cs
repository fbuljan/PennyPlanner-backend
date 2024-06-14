using AutoMapper;
using PennyPlanner.DTOs.Accounts;
using PennyPlanner.DTOs.Transactions;
using PennyPlanner.DTOs.User;
using PennyPlanner.Exceptions;
using PennyPlanner.Models;
using PennyPlanner.Repository;
using PennyPlanner.Services.Interfaces;

namespace PennyPlanner.Services
{
    public class AccountService : IAccountService
    {
        private IMapper Mapper { get; }
        private IGenericRepository<Account> AccountRepository { get; }
        private IGenericRepository<User> UserRepository { get; }
        private ITransactionService TransactionService { get; }

        public AccountService(IMapper mapper, IGenericRepository<Account> accountRepository, 
            IGenericRepository<User> userRepository, ITransactionService transactionService)
        {
            Mapper = mapper;
            AccountRepository = accountRepository;
            UserRepository = userRepository;
            TransactionService = transactionService;
        }

        public async Task<int> CreateAccountAsync(AccountCreate accountCreate)
        {
            var user = await UserRepository.GetByIdAsync(accountCreate.UserId, u => u.Accounts) 
                ?? throw new UserNotFoundException(accountCreate.UserId);
            var account = Mapper.Map<Account>(accountCreate);

            foreach (var otherAccount in user.Accounts)
            {
                if (otherAccount.Name == account.Name) 
                    throw new AccountNameAlreadyInUseException(user.Id, account.Name);
            }

            account.User = user;
            user.Accounts.Add(account);

            await AccountRepository.InsertAsync(account);
            await AccountRepository.SaveChangesAsync();

            return account.Id;
        }

        public async Task UpdateAccountAsync(AccountUpdate accountUpdate)
        {
            var account = await AccountRepository.GetByIdAsync(accountUpdate.Id, a => a.User, a => a.User.Accounts) ?? throw new AccountNotFoundException(accountUpdate.Id);

            if (!string.IsNullOrWhiteSpace(accountUpdate.Name) && accountUpdate.Name != account.Name)
            {
                var user = account.User;
                foreach (var otherAccount in user.Accounts)
                {
                    await Console.Out.WriteLineAsync(otherAccount.Name);
                    if (otherAccount.Name == accountUpdate.Name)
                        throw new AccountNameAlreadyInUseException(user.Id, otherAccount.Name);
                }
                account.Name = accountUpdate.Name;
            }

            if (!string.IsNullOrWhiteSpace(accountUpdate.Description))
                account.Description = accountUpdate.Description;
            
            AccountRepository.Update(account);
            await AccountRepository.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(AccountDelete accountDelete)
        {
            var account = await AccountRepository.GetByIdAsync(accountDelete.Id, account => account.Transactions) ?? throw new AccountNotFoundException(accountDelete.Id);
            var transactionsToDelete = new List<int>();

            foreach (var transaction in account.Transactions)
            {
                transactionsToDelete.Add(transaction.Id);
            }

            foreach (var id in transactionsToDelete)
            {
                await TransactionService.DeleteTransactionAsync(new TransactionDelete { Id = id });
            }

            AccountRepository.Delete(account);
            await AccountRepository.SaveChangesAsync();
        }

        public async Task<AccountGet> GetAccountAsync(int id)
        {
            var account = await AccountRepository.GetByIdAsync(id, a => a.Transactions) ?? throw new AccountNotFoundException(id);
            return Mapper.Map<AccountGet>(account);
        }

        public async Task<List<AccountGet>> GetAccountsAsync()
        {
            var accounts = await AccountRepository.GetAsync(null, null, a => a.Transactions);
            return Mapper.Map<List<AccountGet>>(accounts);
        }

        public async Task AddTransactionAsync(int accountId, Transaction transaction)
        {
            var account = await AccountRepository.GetByIdAsync(accountId, a => a.Transactions) ?? throw new AccountNotFoundException(accountId);
            account.Transactions.Add(transaction);
            await AccountRepository.SaveChangesAsync();
        }
    }
}
