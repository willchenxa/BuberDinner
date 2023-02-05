using ErrorOr;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.User;

namespace BuberDinner.Application.Services.Authentication.Queries;

public class AuthenticationQueryService : IAuthenticationQueryService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationQueryService(
     IJwtTokenGenerator jtwTokenGenerator,
     IUserRepository userRepository)
    {
        _jwtTokenGenerator = jtwTokenGenerator;
        _userRepository = userRepository;
    }

    public ErrorOr<AuthenticationResult> Login(string email, string password)
    {
        // validate the user exists
        if (_userRepository.GetUserByEmail(email) is not User user)
        {
            //throw new Exception("User with given email does not exist.");
            return Errors.Authentication.InvalidCredential;
        }

        // validate the password is correct
        if (user.Password != password)
        {
            //throw new Exception("Invalid password");
            return new[] { Errors.Authentication.InvalidCredential };
        }

        // create JWT token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user,
                                        token);
    }
}