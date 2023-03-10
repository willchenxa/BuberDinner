using ErrorOr;
using BuberDinner.Application.Authentication.Common;

namespace BuberDinner.Application.Services.Authentication.Queries;

public interface IAuthenticationQueryService
{
    Task<ErrorOr<AuthenticationResult>> Login(string email, string password);
}