using PennyPlanner.DTOs.Goals;

namespace PennyPlanner.Services.Interfaces
{
    public interface IGoalService
    {
        Task<int> CreateGoalAsync(GoalCreate goalCreate);
        Task UpdateGoalAsync(GoalUpdate goalUpdate);
        Task DeleteGoalAsync(GoalDelete goalDelete);
        Task<GoalGet> GetGoalAsync(int id);
        Task<List<GoalGet>> GetGoalsAsync();
    }
}
