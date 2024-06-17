using AutoMapper;
using PennyPlanner.DTOs.Accounts;
using PennyPlanner.DTOs.Goals;
using PennyPlanner.DTOs.Transactions;
using PennyPlanner.DTOs.User;
using PennyPlanner.Models;

namespace PennyPlanner.Mapping
{
    public class DtoEntityMapperProfile : Profile
    {
        public DtoEntityMapperProfile()
        {
            CreateMap<UserCreate, User>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<UserUpdate, User>();
            CreateMap<User, UserGet>();

            CreateMap<AccountCreate, Account>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<AccountUpdate, Account>();
            CreateMap<Account, AccountGet>();

            CreateMap<TransactionCreate, Transaction>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<TransactionUpdate, Transaction>();
            CreateMap<Transaction, TransactionGet>();

            CreateMap<GoalCreate, Goal>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<GoalUpdate, Goal>();
            CreateMap<Goal, GoalGet>();
        }
    }
}
