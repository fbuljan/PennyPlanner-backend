using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PennyPlanner.DTOs.Goals;
using PennyPlanner.Enums;
using PennyPlanner.Services.Interfaces;
using PennyPlanner.Utils;

namespace PennyPlanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoalController : ControllerBase
    {
        private IGoalService GoalService { get; }

        public GoalController(IGoalService goalService)
        {
            GoalService = goalService;
        }

        //[Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateGoal(GoalCreate goalCreate)
        {
            var id = await GoalService.CreateGoalAsync(goalCreate);
            var goal = await GoalService.GetGoalAsync(id);
            var response = new { success = true, goal };

            return Ok(response);
        }

        //[Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateGoal(GoalUpdate goalUpdate)
        {
            await GoalService.UpdateGoalAsync(goalUpdate);
            var goal = await GoalService.GetGoalAsync(goalUpdate.Id);

            return Ok(goal);
        }

        //[Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteGoal(GoalDelete goalDelete)
        {
            await GoalService.DeleteGoalAsync(goalDelete);
            return Ok();
        }

        //[Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetGoal(int id)
        {
            var goal = await GoalService.GetGoalAsync(id);
            return Ok(goal);
        }

        //[Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetGoals()
        {
            var goals = await GoalService.GetGoalsAsync();
            return Ok(goals);
        }

        //[Authorize]
        [HttpGet("types")]
        public IActionResult GetGoalTypes()
        {
            var types = Enum.GetValues(typeof(GoalType))
                               .Cast<GoalType>()
                               .Select(e => new { Name = NamingUtils.ConvertToCamelCaseWithSpaces(e.ToString()), Value = (int)e })
                               .ToList();
            return Ok(types);
        }
    }
}
