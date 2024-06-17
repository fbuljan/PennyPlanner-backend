using AutoMapper;
using FluentValidation;
using PennyPlanner.DTOs.Goals;
using PennyPlanner.Exceptions;
using PennyPlanner.Models;
using PennyPlanner.Repository;
using PennyPlanner.Services.Interfaces;
using PennyPlanner.Validation;

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
            goal.CurrentValue = 0;
            goal.StartDate = DateTime.Now;

            if (goalCreate.AccountId.HasValue)
            {
                goal.Account = await AccountRepository.GetByIdAsync(goalCreate.AccountId.Value)
                    ?? throw new AccountNotFoundException(goalCreate.AccountId.Value);
            }

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

            var goal = await GoalRepository.GetByIdAsync(goalUpdate.Id)
                ?? throw new GoalNotFoundException(goalUpdate.Id);

            Mapper.Map(goalUpdate, goal);
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
            var goals = await GoalRepository.GetAsync(null, null, g => g.Account!);
            return Mapper.Map<List<GoalGet>>(goals);
        }
    }
}
