using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
 private readonly IAuthenticationService _authenticationService;

 public AuthenticationController(IAuthenticationService service)
 {
  _authenticationService = service;
 }

 [Route("register")]
 public IActionResult Register(RegisterRequest request)
 {
  ErrorOr<AuthenticationResult> authResult = _authenticationService.Register(
      request.FirstName,
      request.LastName,
      request.Email,
      request.Password);

  return authResult.Match(
    authResult => Ok(MapAuthResult(authResult)),
    errors => Problem(errors)
  );

  // FluentResult solution
  // Result<AuthenticationResult> registerResult = _authenticationService.Register(
  // request.FirstName, 
  // request.LastName, 
  // request.Email, 
  // request.Password);

  // if (registerResult.IsSuccess)
  // {
  //  return Ok(MapAuthResult(registerResult.Value));
  // }

  // var firstError = registerResult.Errors[0];
  // if (firstError is DuplicateEmailError)
  // {
  //  return Problem(statusCode: StatusCodes.Status409Conflict, title: "Email already exists.");
  // }

  // return Problem();

  // oneof package solution
  // OneOf<AuthenticationResult, IError> registerResult = _authenticationService.Register(request.FirstName, request.LastName, request.Email, request.Password);
  // return registerResult.Match(
  //  authResult => Ok(MapAuthResult(authResult)),
  //  error => Problem(statusCode: (int)error.StatusCode, title: error.ErrorMessage));

  // Not too readable solution
  // if (registerResult.IsT0)
  // {
  //  var authResult = registerResult.AsT0;
  //  MapAuthResult(authResult)

  //  return Ok(response);
  // }
  // return Problem(statusCode: StatusCodes.Status409Conflict, title: "Email already exists.");
 }

 [Route("login")]
 public IActionResult Login(LoginRequest request)
 {
  ErrorOr<AuthenticationResult> authResult = _authenticationService.Login(
    request.Email,
    request.Password);

  if (authResult.IsError && authResult.FirstError == Domain.Common.Errors.Errors.Authentication.InvalidCredential)
  {
   return Problem(
     statusCode: StatusCodes.Status401Unauthorized,
     title: authResult.FirstError.Description
   );
  }
  return authResult.Match(
    authResult => Ok(MapAuthResult(authResult)),
    errors => Problem(errors)
  );

  // var authResult = _authenticationService.Login(request.Email, request.Password);

  // var response = new AuthenticationResponse(
  // authResult.User.Id,
  // authResult.User.FirstName,
  // authResult.User.LastName,
  // authResult.User.Email,
  // authResult.Token);

  // return Ok(response);
 }

 private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
 {
  return new AuthenticationResponse(
   authResult.User.Id,
   authResult.User.FirstName,
   authResult.User.LastName,
   authResult.User.Email,
   authResult.Token);
 }
}