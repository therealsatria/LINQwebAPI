using Infrastructure.DTOs;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    
    public AuthController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (registerDto.Password != registerDto.ConfirmPassword)
            return BadRequest("Passwords do not match");
            
        var user = await _userService.RegisterAsync(registerDto);
        
        if (user == null)
            return BadRequest("Username or email already exists");
            
        return Ok(user);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userService.LoginAsync(loginDto);
        
        if (user == null)
            return Unauthorized("Invalid username or password");
            
        return Ok(user);
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userId = User.FindFirst("nameid")?.Value;
        
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid id))
            return Unauthorized();
            
        var user = await _userService.GetUserByIdAsync(id);
        
        if (user == null)
            return NotFound();
            
        return Ok(user);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }
} 