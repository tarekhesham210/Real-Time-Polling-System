using Domain.Entities;

namespace Application.Interfaces.JWT
{
    public interface IJwtProvider
    {
        (string Value,  DateTime ExpiryInMinutes) GenerateToken(ApplicationUser user);
        string GenerateRefreshToken();
    }
}
