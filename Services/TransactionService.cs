using AutoMapper;
using FluentValidation;
using PennyPlanner.DTOs.Transactions;
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
        private TransactionCreateValidator TransactionCreateValidator { get; }
        private TransactionUpdateValidator TransactionUpdateValidator { get; }

        public TransactionService(IMapper mapper, IGenericRepository<Transaction> transactionRepository,
            IGenericRepository<Account> accountRepository, TransactionCreateValidator transactionCreateValidator, TransactionUpdateValidator transactionUpdateValidator)
        {
            Mapper = mapper;
            TransactionRepository = transactionRepository;
            AccountRepository = accountRepository;
            TransactionCreateValidator = transactionCreateValidator;
            TransactionUpdateValidator = transactionUpdateValidator;
        }

        public async Task<int> CreateTransactionAsync(TransactionCreate transactionCreate)
        {
            await TransactionCreateValidator.ValidateAndThrowAsync(transactionCreate);

            var account = await AccountRepository.GetByIdAsync(transactionCreate.AccountId, a => a.Transactions, a => a.User.Transactions)
                ?? throw new AccountNotFoundException(transactionCreate.AccountId);

            var transaction = Mapper.Map<Transaction>(transactionCreate);
            transaction.Account = account;
            account.Transactions.Add(transaction);
            transaction.User = account.User;
            account.User.Transactions.Add(transaction);
            ApplyTransaction(account, transaction);
            
            await TransactionRepository.InsertAsync(transaction);
            await TransactionRepository.SaveChangesAsync();

            return transaction.Id;
        }

        public async Task UpdateTransactionAsync(TransactionUpdate transactionUpdate)
        {
            await TransactionUpdateValidator.ValidateAndThrowAsync(transactionUpdate);

            var transaction = await TransactionRepository.GetByIdAsync(transactionUpdate.Id, t => t.Account)
                ?? throw new TransactionNotFoundException(transactionUpdate.Id);
            var account = transaction.Account;

            if (transactionUpdate.Amount.HasValue && transactionUpdate.Amount.Value != transaction.Amount)
            {
                ApplyTransaction(account, transaction, true);
                transaction.Amount = transactionUpdate.Amount.Value;
                ApplyTransaction(account, transaction);
            }

            if (transactionUpdate.TransactionType.HasValue && transactionUpdate.TransactionType.Value != transaction.TransactionType)
            {
                ApplyTransaction(account, transaction, true);
                transaction.TransactionType = transactionUpdate.TransactionType.Value;
                ApplyTransaction(account, transaction);
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
            var transaction = await TransactionRepository.GetByIdAsync(transactionDelete.Id, t => t.Account)
                ?? throw new TransactionNotFoundException(transactionDelete.Id);

            if (transactionDelete.ResetAmount) ApplyTransaction(transaction.Account, transaction, true);

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
            var transactions = await TransactionRepository.GetAsync(null, null);
            return Mapper.Map<List<TransactionGet>>(transactions);
        }

        private void ApplyTransaction(Account account, Transaction transaction, bool reverse = false)
        {
            int reverseMultiplier = reverse ? -1 : 1;
            account.Balance += transaction.Amount * (int)transaction.TransactionType * reverseMultiplier;
        }
    }
}
