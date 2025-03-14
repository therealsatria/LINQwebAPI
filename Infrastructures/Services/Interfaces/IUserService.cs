using Infrastructure.DTOs;

namespace Infrastructure.Services;
public interface IUserService
{
    Task<UserDto?> LoginAsync(LoginDto loginDto);
    Task<UserDto?> RegisterAsync(RegisterDto registerDto);
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
}