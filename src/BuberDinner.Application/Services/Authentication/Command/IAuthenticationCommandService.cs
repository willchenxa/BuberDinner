using ErrorOr;
using BuberDinner.Application.Authentication.Common;

namespace BuberDinner.Application.Services.Authentication;

public interface IAuthenticationCommandService
{
    ErrorOr<AuthenticationResult> Register(string firstName,
                                           string lastName,
                                           string email,
                                           string password);
}