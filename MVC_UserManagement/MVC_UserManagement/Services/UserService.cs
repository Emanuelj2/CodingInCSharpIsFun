using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MVC_UserManagement.Data;
using MVC_UserManagement.Models;

namespace MVC_UserManagement.Services
{
    public interface IUserService
    {
        //get all users
        Task<IEnumerable<UserModel>> GetAllUsersAsync();
        //get user by id
        Task<UserModel> GetUserByIdAsync(int id);
        //add a user
        Task<UserModel> AddUserAsync(UserModel user);
        //update a user
        Task<UserModel> UpdateUserAsync(UserModel user);
        //delete a user
        Task DeleteUserAsync(int id);

    }

    public class UserService : IUserService
    {
        public readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel> AddUserAsync(UserModel user)
        {
            //if the user exist throw an error
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);

            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }

            //create user
            _context.Users.Add(user);
            await _context.SaveChangesAsync(); //save the changes to the database
            return user;
        }

        public async Task DeleteUserAsync(int id)
        {
            //check if the user exists
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            _context.Users.Remove(user); //remove the user from the database
            await _context.SaveChangesAsync(); //save the changes to the database
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                throw new KeyNotFoundException("User not found.");
            }
            
            return await Task.FromResult(user);
        }

        public async Task<UserModel> UpdateUserAsync(UserModel user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            //update user details
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;

            await _context.SaveChangesAsync(); //save the changes to the database
            return existingUser;

        }
    }
}
