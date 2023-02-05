namespace BuberDinner.Application.Common.Interfaces.Authentication;

using BuberDinner.Domain.User;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}