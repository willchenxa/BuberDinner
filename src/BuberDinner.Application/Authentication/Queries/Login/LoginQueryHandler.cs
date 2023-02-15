using ErrorOr;
using MediatR;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.User;

namespace BuberDinner.Application.Authentication.Queries.Login;

public class LoginQueryHandler :
 IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginQueryHandler(
     IJwtTokenGenerator jtwTokenGenerator,
     IUserRepository userRepository)
    {
        _jwtTokenGenerator = jtwTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        // validate the user exists
        if (await _userRepository.GetUserByEmail(query.Email) is not User user)
        {
            //throw new Exception("User with given email does not exist.");
            return Errors.Authentication.InvalidCredential;
        }

        // validate the password is correct
        if (user.Password != query.Password)
        {
            //throw new Exception("Invalid password");
            return new[] { Errors.Authentication.InvalidCredential };
        }

        // create JWT token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(
         user,
         token);
    }
}