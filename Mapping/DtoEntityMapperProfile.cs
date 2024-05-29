using AutoMapper;
using PennyPlanner.DTOs.Account;
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
        }
    }
}
