using Infrastructure.DTOs;
using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public interface IUserService
{
    Task<UserDto?> LoginAsync(LoginDto loginDto);
    Task<UserDto?> RegisterAsync(RegisterDto registerDto);
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    
    public UserService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }
    
    public async Task<UserDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.LoginAsync(loginDto.Username, loginDto.Password);
        
        if (user == null) return null;
        
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            Token = _tokenService.CreateToken(user)
        };
    }
    
    public async Task<UserDto?> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userRepository.UserExistsAsync(registerDto.Username))
            return null;
            
        if (await _userRepository.EmailExistsAsync(registerDto.Email))
            return null;
            
        var user = await _userRepository.RegisterUserAsync(
            registerDto.Username,
            registerDto.Email,
            registerDto.Password
        );
        
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            Token = _tokenService.CreateToken(user)
        };
    }
    
    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        
        if (user == null) return null;
        
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            Token = _tokenService.CreateToken(user)
        };
    }
    
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        
        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            Token = string.Empty // Don't generate tokens for listing
        });
    }
} 