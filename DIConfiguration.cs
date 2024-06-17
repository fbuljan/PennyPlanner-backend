using PennyPlanner.Mapping;
using PennyPlanner.Services;
using PennyPlanner.Services.Interfaces;
using PennyPlanner.Validation;

namespace PennyPlanner
{
    public class DIConfiguration
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DtoEntityMapperProfile));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IGoalService, GoalService>();

            services.AddScoped<UserCreateValidator>();
            services.AddScoped<UserUpdateValidator>();
            services.AddScoped<TransactionCreateValidator>();
            services.AddScoped<TransactionUpdateValidator>();
            services.AddScoped<GoalCreateValidator>();
            services.AddScoped<GoalUpdateValidator>();
        }
    }
}
