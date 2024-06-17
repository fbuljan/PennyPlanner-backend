using AutoMapper;
using FluentValidation;
using PennyPlanner.DTOs.Accounts;
using PennyPlanner.DTOs.User;
using PennyPlanner.Exceptions;
using PennyPlanner.Models;
using PennyPlanner.Repository;
using PennyPlanner.Services.Interfaces;
using PennyPlanner.Utils;
using PennyPlanner.Validation;
using System.Security.Principal;

namespace PennyPlanner.Services
{
    public class UserService : IUserService
    {
        private IMapper Mapper { get; }
        private IGenericRepository<User> UserRepository { get; }
        private IAccountService AccountService { get; }
        private UserCreateValidator UserCreateValidator { get; }
        private UserUpdateValidator UserUpdateValidator { get; }

        public UserService(IMapper mapper, IGenericRepository<User> userRepository, IAccountService accountService, 
            UserCreateValidator userCreateValidator, UserUpdateValidator userUpdateValidator)
        {
            Mapper = mapper;
            UserRepository = userRepository;
            AccountService = accountService;
            UserCreateValidator = userCreateValidator;
            UserUpdateValidator = userUpdateValidator;
        }

        public async Task<int> CreateUserAsync(UserCreate userCreate)
        {
            await UserCreateValidator.ValidateAndThrowAsync(userCreate);

            var users = await UserRepository.GetAsync(null, null);

            if (users.Any(u => u.Username == userCreate.Username))
                throw new UserAlreadyExistsException($"Username '{userCreate.Username}' is already taken.");

            if (users.Any(u => u.Email == userCreate.Email))
                throw new UserAlreadyExistsException($"Email '{userCreate.Email}' is already in use.");

            var user = Mapper.Map<User>(userCreate);
            user.RegistrationDate = DateTime.Now;
            user.Password = PasswordUtils.HashPassword(user.Password);
            await UserRepository.InsertAsync(user);
            await UserRepository.SaveChangesAsync();

            return user.Id;
        }

        public async Task DeleteUserAsync(UserDelete userDelete)
        {
            var user = await UserRepository.GetByIdAsync(userDelete.Id, user => user.Accounts) ?? throw new UserNotFoundException(userDelete.Id);
            var accountsToDelete = new List<int>();

            foreach (var account in user.Accounts)
            {
                accountsToDelete.Add(account.Id);
            }

            foreach (var id in accountsToDelete)
            {
                await AccountService.DeleteAccountAsync(new AccountDelete { Id = id });
            }

            UserRepository.Delete(user);
            await UserRepository.SaveChangesAsync();
        }

        public async Task<UserGet> GetUserAsync(int id)
        {
            var user = await UserRepository.GetByIdAsync(id, u => u.Accounts, u => u.Transactions, u => u.Goals) 
                ?? throw new UserNotFoundException(id);

            return Mapper.Map<UserGet>(user);
        }

        public async Task<List<UserGet>> GetUsersAsync()
        {
            var users = await UserRepository.GetAsync(null, null, u => u.Accounts, u => u.Transactions, u => u.Goals);
            return Mapper.Map<List<UserGet>>(users);
        }

        public async Task UpdateUserAsync(UserUpdate userUpdate)
        {
            await UserUpdateValidator.ValidateAndThrowAsync(userUpdate);

            var existingUser = await UserRepository.GetByIdAsync(userUpdate.Id) ?? throw new UserNotFoundException(userUpdate.Id);

            var users = await UserRepository.GetAsync(null, null);

            if (!string.IsNullOrWhiteSpace(userUpdate.Username) && userUpdate.Username != existingUser.Username)
            {
                if (users.Any(user => user.Username == userUpdate.Username))
                    throw new UserAlreadyExistsException($"Username '{userUpdate.Username}' is already taken.");
                existingUser.Username = userUpdate.Username;
            }

            if (!string.IsNullOrWhiteSpace(userUpdate.Email) && userUpdate.Email != existingUser.Email)
            {
                if (users.Any(user => user.Email == userUpdate.Email))
                    throw new UserAlreadyExistsException($"Email '{userUpdate.Email}' is already taken.");
                existingUser.Email = userUpdate.Email;
            }

            if (!string.IsNullOrWhiteSpace(userUpdate.Password))
                existingUser.Password = PasswordUtils.HashPassword(userUpdate.Password);

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
