using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using FluentResults;
using OneOf;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
 private readonly IJwtTokenGenerator _jwtTokenGenerator;
 private readonly IUserRepository _userRepository;

 public AuthenticationService(
  IJwtTokenGenerator jtwTokenGenerator,
  IUserRepository userRepository)
 {
  _jwtTokenGenerator = jtwTokenGenerator;
  _userRepository = userRepository;
 }

 //public OneOf<AuthenticationResult, IError> Register(string firstName, string lastName, string email, string password)
 //public Result<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
 public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
 {
  // validate the user doesn't exist
  if (_userRepository.GetUserByEmail(email) is not null)
  {
   // when use error endpoint
   //throw new DuplicateEmailException();

   // when use oneof
   //return new DuplicateEmailError();

   // when use FluentResults
   //return FluentResults.Result.Fail(new[] {new DuplicateEmailError()});

   return Errors.User.DuplicateEmail;
  }
  // Create user (generate unique ID) & Persist to DB
  var user = new User
  {
   FirstName = firstName,
   LastName = lastName,
   Email = email,
   Password = password
  };

  _userRepository.Add(user);

  // Create JWT token
  var userId = Guid.NewGuid();
  var token = _jwtTokenGenerator.GenerateToken(user);

  return new AuthenticationResult(
   user,
   token);
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

  return new AuthenticationResult(
   user,
   token);
 }
}