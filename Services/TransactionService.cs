using AutoMapper;
using FluentValidation;
using PennyPlanner.DTOs.Goals;
using PennyPlanner.DTOs.Transactions;
using PennyPlanner.Enums;
using PennyPlanner.Exceptions;
using PennyPlanner.Models;
using PennyPlanner.Repository;
using PennyPlanner.Services.Interfaces;
using PennyPlanner.Validation;

namespace PennyPlanner.Services
{
    public class TransactionService : ITransactionService
    {
        private IMapper Mapper { get; }
        private IGenericRepository<Transaction> TransactionRepository { get; }
        private IGenericRepository<Account> AccountRepository { get; }
        private IGoalService GoalService { get; }
        private TransactionCreateValidator TransactionCreateValidator { get; }
        private TransactionUpdateValidator TransactionUpdateValidator { get; }

        public TransactionService(IMapper mapper, IGenericRepository<Transaction> transactionRepository, IGenericRepository<Account> accountRepository, 
            IGoalService goalService, TransactionCreateValidator transactionCreateValidator, TransactionUpdateValidator transactionUpdateValidator)
        {
            Mapper = mapper;
            TransactionRepository = transactionRepository;
            AccountRepository = accountRepository;
            GoalService = goalService;
            TransactionCreateValidator = transactionCreateValidator;
            TransactionUpdateValidator = transactionUpdateValidator;
        }

        public async Task<int> CreateTransactionAsync(TransactionCreate transactionCreate, bool save)
        {
            await TransactionCreateValidator.ValidateAndThrowAsync(transactionCreate);

            var account = await AccountRepository.GetByIdAsync(transactionCreate.AccountId, a => a.Transactions, a => a.User.Transactions)
                ?? throw new AccountNotFoundException(transactionCreate.AccountId);

            var transaction = Mapper.Map<Transaction>(transactionCreate);
            transaction.Account = account;
            account.Transactions.Add(transaction);
            transaction.User = account.User;
            account.User.Transactions.Add(transaction);
            await ApplyTransaction(account, transaction, transaction.User, false, !save || transactionCreate.OtherAccountId != null);

            if (transactionCreate.OtherAccountId.HasValue && transactionCreate.TransactionType != TransactionType.Template)
            {
                transaction.OtherAccountId = transactionCreate.OtherAccountId.Value;
                await CreateTransactionAsync(new TransactionCreate
                {
                    AccountId = transactionCreate.OtherAccountId.Value,
                    Date = transactionCreate.Date,
                    Amount = transactionCreate.Amount,
                    TransactionType = (TransactionType)(-(int)transactionCreate.TransactionType),
                    TransactionCategory = transactionCreate.TransactionCategory,
                    Description = transactionCreate.Description
                }, false);
            }
            
            await TransactionRepository.InsertAsync(transaction);
            if (save) await TransactionRepository.SaveChangesAsync();

            return transaction.Id;
        }

        public async Task UpdateTransactionAsync(TransactionUpdate transactionUpdate)
        {
            await TransactionUpdateValidator.ValidateAndThrowAsync(transactionUpdate);

            var transaction = await TransactionRepository.GetByIdAsync(transactionUpdate.Id, t => t.Account, t => t.User)
                ?? throw new TransactionNotFoundException(transactionUpdate.Id);
            var account = transaction.Account;
            var user = transaction.User;

            if (transactionUpdate.TransactionType.HasValue && transactionUpdate.TransactionType.Value != transaction.TransactionType)
            {
                await ApplyTransaction(account, transaction, user, true);
                transaction.TransactionType = transactionUpdate.TransactionType.Value;
                await ApplyTransaction(account, transaction, user);
            }

            if (transactionUpdate.Amount.HasValue && transactionUpdate.Amount.Value != transaction.Amount)
            {
                await ApplyTransaction(account, transaction, user, true);
                transaction.Amount = transactionUpdate.Amount.Value;
                await ApplyTransaction(account, transaction, user);
            }

            if (transactionUpdate.TransactionCategory.HasValue)
                transaction.TransactionCategory = transactionUpdate.TransactionCategory.Value;

            if (transactionUpdate.Date.HasValue)
                transaction.Date = transactionUpdate.Date.Value;

            if (!string.IsNullOrEmpty(transactionUpdate.Description))
                transaction.Description = transactionUpdate.Description;

            TransactionRepository.Update(transaction);
            await TransactionRepository.SaveChangesAsync();
        }

        public async Task DeleteTransactionAsync(TransactionDelete transactionDelete)
        {
            var transaction = await TransactionRepository.GetByIdAsync(transactionDelete.Id, t => t.Account, t => t.User)
                ?? throw new TransactionNotFoundException(transactionDelete.Id);

            if (transactionDelete.ResetAmount) 
                await ApplyTransaction(transaction.Account, transaction, transaction.User, true);

            TransactionRepository.Delete(transaction);
            await TransactionRepository.SaveChangesAsync();
        }

        public async Task<TransactionGet> GetTransactionAsync(int id)
        {
            var transaction = await TransactionRepository.GetByIdAsync(id) ?? throw new TransactionNotFoundException(id);
            return Mapper.Map<TransactionGet>(transaction);
        }

        public async Task<List<TransactionGet>> GetTransactionsAsync()
        {
            var transactions = await TransactionRepository.GetAsync(null, null, null);
            return Mapper.Map<List<TransactionGet>>(transactions);
        }

        private async Task ApplyTransaction(Account account, Transaction transaction, User user, bool reverse = false, bool internalTransaction = false)
        {
            int reverseMultiplier = reverse ? -1 : 1;
            int amount = transaction.Amount * (int)transaction.TransactionType * reverseMultiplier;
            account.Balance += amount;
            await GoalService.UpdateGoalsProgress(new GoalUpdateProgress
            {
                User = user,
                Account = account,
                Transaction = transaction,
                Amount = amount,
                InternalTransaction = internalTransaction
            });
        }
    }
}
