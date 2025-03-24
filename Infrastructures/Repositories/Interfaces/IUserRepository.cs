using Infrastructure.Models;

namespace Infrastructure.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> UserExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
    Task<User> RegisterUserAsync(string username, string email, string password);
    Task<User?> LoginAsync(string username, string password);
    Task<IEnumerable<User>> GetAllUsersAsync();
}