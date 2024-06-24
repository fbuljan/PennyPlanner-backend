using AutoMapper;
using FluentValidation;
using PennyPlanner.DTOs.Goals;
using PennyPlanner.Enums;
using PennyPlanner.Exceptions;
using PennyPlanner.Models;
using PennyPlanner.Repository;
using PennyPlanner.Services.Interfaces;
using PennyPlanner.Validation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PennyPlanner.Services
{
    public class GoalService : IGoalService
    {
        private IMapper Mapper { get; }
        private IGenericRepository<Goal> GoalRepository { get; }
        private IGenericRepository<Account> AccountRepository { get; }
        private IGenericRepository<User> UserRepository { get; }
        private GoalCreateValidator GoalCreateValidator { get; }
        private GoalUpdateValidator GoalUpdateValidator { get; }

        public GoalService(IMapper mapper, IGenericRepository<Goal> goalRepository, IGenericRepository<Account> accountRepository,
            IGenericRepository<User> userRepository, GoalCreateValidator goalCreateValidator, GoalUpdateValidator goalUpdateValidator)
        {
            Mapper = mapper;
            GoalRepository = goalRepository;
            AccountRepository = accountRepository;
            UserRepository = userRepository;
            GoalCreateValidator = goalCreateValidator;
            GoalUpdateValidator = goalUpdateValidator;
        }

        public async Task<int> CreateGoalAsync(GoalCreate goalCreate)
        {
            await GoalCreateValidator.ValidateAndThrowAsync(goalCreate);

            var goal = Mapper.Map<Goal>(goalCreate);
            goal.StartDate = DateTime.Now;

            if (goalCreate.GoalType == GoalType.AccountMoney && goalCreate.AccountId.HasValue)
            {
                goal.Account = await AccountRepository.GetByIdAsync(goalCreate.AccountId.Value)
                    ?? throw new AccountNotFoundException(goalCreate.AccountId.Value);

                goal.CurrentValue = goal.Account.Balance;
            }
            else if (goal.GoalType == GoalType.TotalMoney)
            {
                goal.CurrentValue = await GetUserTotalBalanceAsync(goalCreate.UserId);
            }
            else if (goal.GoalType is GoalType.MonthlyIncome or GoalType.MonthlyExpenseReduction)
            {
                goal.CurrentValue = 0;
                goal.EndDate = DateTime.Now.AddMonths(1);
            }

            goal.IsAchieved = goal.GoalType == GoalType.MonthlyExpenseReduction ?
                goal.CurrentValue <= goal.TargetValue : goal.CurrentValue >= goal.TargetValue;

            var user = await UserRepository.GetByIdAsync(goalCreate.UserId, u => u.Goals)
                ?? throw new UserNotFoundException(goalCreate.UserId);

            goal.User = user;
            user.Goals.Add(goal);

            await GoalRepository.InsertAsync(goal);
            await GoalRepository.SaveChangesAsync();

            return goal.Id;
        }

        public async Task UpdateGoalAsync(GoalUpdate goalUpdate)
        {
            await GoalUpdateValidator.ValidateAndThrowAsync(goalUpdate);

            var goal = await GoalRepository.GetByIdAsync(goalUpdate.Id) ?? throw new GoalNotFoundException(goalUpdate.Id);

            if (!string.IsNullOrEmpty(goalUpdate.Name))
            {
                goal.Name = goalUpdate.Name;
            }

            if (goalUpdate.EndDate.HasValue)
            {
                goal.EndDate = goalUpdate.EndDate.Value;
            }

            if (goalUpdate.TargetValue.HasValue)
            {
                goal.TargetValue = goalUpdate.TargetValue.Value;
            }

            if (goalUpdate.CurrentValue.HasValue)
            {
                goal.CurrentValue = goalUpdate.CurrentValue.Value;
            }

            GoalRepository.Update(goal);
            await GoalRepository.SaveChangesAsync();
        }

        public async Task DeleteGoalAsync(GoalDelete goalDelete)
        {
            var goal = await GoalRepository.GetByIdAsync(goalDelete.Id)
                ?? throw new GoalNotFoundException(goalDelete.Id);

            GoalRepository.Delete(goal);
            await GoalRepository.SaveChangesAsync();
        }

        public async Task<GoalGet> GetGoalAsync(int id)
        {
            var goal = await GoalRepository.GetByIdAsync(id, g => g.Account!)
                ?? throw new GoalNotFoundException(id);

            return Mapper.Map<GoalGet>(goal);
        }

        public async Task<List<GoalGet>> GetGoalsAsync()
        {
            var goals = await GoalRepository.GetAsync(null, null, null, g => g.Account!);
            return Mapper.Map<List<GoalGet>>(goals);
        }

        public async Task UpdateGoalsProgress(GoalUpdateProgress goalUpdateProgress)
        {
            if (goalUpdateProgress.Transaction.TransactionType == TransactionType.Template) return;

            var userGoals = await GoalRepository.GetAsync(null, null,
                g => g.User.Id == goalUpdateProgress.User.Id, g => g.Account!, g => g.User);

            foreach (var goal in userGoals)
            {
                if (goal.GoalType == GoalType.TotalMoney)
                {
                    goal.CurrentValue += goalUpdateProgress.Amount;
                }
                else if (goal.GoalType == GoalType.AccountMoney)
                {
                    if (goal.Account == null || goal.Account.Id != goalUpdateProgress.Account.Id) continue;

                    goal.CurrentValue += goalUpdateProgress.Amount;
                }
                else if (goal.GoalType == GoalType.MonthlyIncome)
                {
                    if (goalUpdateProgress.Transaction.TransactionType != TransactionType.Income || goalUpdateProgress.Transaction.Date.AddDays(1) < goal.StartDate || goalUpdateProgress.InternalTransaction) continue;

                    goal.CurrentValue += goalUpdateProgress.Amount;
                }
                else if (goal.GoalType == GoalType.MonthlyExpenseReduction)
                {
                    if (goalUpdateProgress.Transaction.TransactionType != TransactionType.Expense || goalUpdateProgress.Transaction.Date.AddDays(1) < goal.StartDate || goalUpdateProgress.InternalTransaction) continue;

                    goal.CurrentValue += -goalUpdateProgress.Amount;
                }

                goal.IsAchieved = goal.GoalType == GoalType.MonthlyExpenseReduction ?
                    goal.CurrentValue <= goal.TargetValue : goal.CurrentValue >= goal.TargetValue;

                GoalRepository.Update(goal);
            }
        }

        private async Task<float> GetUserTotalBalanceAsync(int userId)
        {
            float totalBalance = 0;
            var user = await UserRepository.GetByIdAsync(userId, u => u.Accounts) ?? throw new UserNotFoundException(userId);

            foreach (var account in user.Accounts)
            {
                totalBalance += account.Balance;
            }

            return totalBalance;
        }
    }
}
