using PennyPlanner.DTOs.Transactions;

namespace PennyPlanner.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<int> CreateTransactionAsync(TransactionCreate transactionCreate, bool save);
        Task UpdateTransactionAsync(TransactionUpdate transactionUpdate);
        Task DeleteTransactionAsync(TransactionDelete transactionDelete);
        Task<TransactionGet> GetTransactionAsync(int id);
        Task<List<TransactionGet>> GetTransactionsAsync();
    }
}
