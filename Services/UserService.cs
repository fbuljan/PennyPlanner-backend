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

        public async Task UpdateUserAsync(UserUpdate userUpdate)
        {
            var existingUser = await UserRepository.GetByIdAsync(userUpdate.Id) ?? throw new Exception("User not found");
            
            if (!string.IsNullOrWhiteSpace(userUpdate.Username)) 
                existingUser.Username = userUpdate.Username;

            if (!string.IsNullOrWhiteSpace(userUpdate.Email))
                existingUser.Email = userUpdate.Email;

            if (!string.IsNullOrWhiteSpace(userUpdate.Password))
                existingUser.Password = PasswordUtils.HashPassword(userUpdate.Password);

            if (userUpdate.GetNotifications.HasValue)
                existingUser.GetNotifications = userUpdate.GetNotifications.Value;

            if (!string.IsNullOrWhiteSpace(userUpdate.Name))
                existingUser.Name = userUpdate.Name;

            UserRepository.Update(existingUser);
            await UserRepository.SaveChangesAsync();
        }

        public async Task<UserGet?> GetUserByLoginAsync(string login)
        {
            var users = await GetUsersAsync();

            if (RegexUtils.IsValidEmail(login))
                return users.FirstOrDefault(u => u.Email == login);
            else
                return users.FirstOrDefault(u => u.Username == login);
        }
    }
}
