using PennyPlanner.Mapping;
using PennyPlanner.Services;
using PennyPlanner.Services.Interfaces;

namespace PennyPlanner
{
    public class DIConfiguration
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DtoEntityMapperProfile));
            services.AddScoped<IUserService, UserService>();
        }
    }
}
