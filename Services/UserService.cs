using AutoMapper;
using PennyPlanner.DTOs.User;
using PennyPlanner.Models;
using PennyPlanner.Repository;
using PennyPlanner.Services.Interfaces;
using PennyPlanner.Utils;

namespace PennyPlanner.Services
{
    public class UserService : IUserService
    {
        private IMapper Mapper { get; }
        private IGenericRepository<User> UserRepository { get; }

        public UserService(IMapper mapper, IGenericRepository<User> userRepository)
        {
            Mapper = mapper;
            UserRepository = userRepository;
        }

        public async Task<int> CreateUserAsync(UserCreate userCreate)
        {
            var user = Mapper.Map<User>(userCreate);
            user.RegistrationDate = DateTime.Now;
            user.Password = PasswordUtils.HashPassword(user.Password);
            await UserRepository.InsertAsync(user);
            await UserRepository.SaveChangesAsync();

            return user.Id;
        }

        public async Task DeleteUserAsync(UserDelete userDelete)
        {
            var user = await UserRepository.GetByIdAsync(userDelete.Id);
            UserRepository.Delete(user);
            await UserRepository.SaveChangesAsync();
        }

        public async Task<UserGet> GetUserAsync(int id)
        {
            var user = await UserRepository.GetByIdAsync(id);
            return Mapper.Map<UserGet>(user);
        }

        public async Task<List<UserGet>> GetUsersAsync()
        {
            var users = await UserRepository.GetAsync(null, null);
            return Mapper.Map<List<UserGet>>(users);
        }

        //todo hash new password, allow partial update
        public async Task UpdateUserAsync(UserUpdate userUpdate)
        {
            var user = Mapper.Map<User>(userUpdate);
            UserRepository.Update(user);
            await UserRepository.SaveChangesAsync();
        }
    }
}
