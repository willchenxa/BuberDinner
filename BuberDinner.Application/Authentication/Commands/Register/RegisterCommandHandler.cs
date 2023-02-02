using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using BuberDinner.Application.Authentication.Common;

namespace BuberDinner.Application.Authentication.Commands.Register;

public class RegisterCommandHandler :
 IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
 private readonly IJwtTokenGenerator _jwtTokenGenerator;
 private readonly IUserRepository _userRepository;

 public RegisterCommandHandler(
  IJwtTokenGenerator jtwTokenGenerator,
  IUserRepository userRepository)
 {
  _jwtTokenGenerator = jtwTokenGenerator;
  _userRepository = userRepository;
 }

 public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
 {
  // validate the user doesn't exist
  if (_userRepository.GetUserByEmail(command.Email) is not null)
  {
   return Errors.User.DuplicateEmail;
  }

  // Create user (generate unique ID) & Persist to DB
  var user = new User
  {
   FirstName = command.FirstName,
   LastName = command.LastName,
   Email = command.Email,
   Password = command.Password
  };

  _userRepository.Add(user);

  // Create JWT token
  var userId = Guid.NewGuid();
  var token = _jwtTokenGenerator.GenerateToken(user);

  return new AuthenticationResult(
   user,
   token);
 }
}