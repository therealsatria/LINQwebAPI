using Infrastructure.Models;

namespace Infrastructure.Services;

public interface ITokenService
{
    string CreateToken(User user);
}