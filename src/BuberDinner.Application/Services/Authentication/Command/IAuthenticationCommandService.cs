using BuberDinner.Application.Authentication.Common;

using ErrorOr;

namespace BuberDinner.Application.Services.Authentication.Command;

public interface IAuthenticationCommandService
{
    Task<ErrorOr<AuthenticationResult>> Register(string firstName,
                                           string lastName,
                                           string email,
                                           string password);
}