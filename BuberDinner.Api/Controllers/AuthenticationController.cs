using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Contracts.Authentication;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
 private readonly ISender _mediator;
    private readonly IMapper _mapper;

 public AuthenticationController(IMediator mediator, IMapper mapper)
 {
  _mediator = mediator;
        _mapper = mapper;
 }

 [Route("register")]
 public async Task<IActionResult> Register(RegisterRequest request)
 {
        var command = _mapper.Map<RegisterCommand>(request);
  ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);

  return authResult.Match(
    authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
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
 public async Task<IActionResult> Login(LoginRequest request)
 {
        var query = _mapper.Map<LoginQuery>(request);

  ErrorOr<AuthenticationResult> authResult = await _mediator.Send(query);

  // ErrorOr<AuthenticationResult> authResult = _authenticationQueryService.Login(
  //   request.Email,
  //   request.Password);

  if (authResult.IsError && authResult.FirstError == Domain.Common.Errors.Errors.Authentication.InvalidCredential)
  {
   return Problem(
     statusCode: StatusCodes.Status401Unauthorized,
     title: authResult.FirstError.Description
   );
  }
  return authResult.Match(
    authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
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

 //private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
 //{
 // return new AuthenticationResponse(
 //  authResult.User.Id,
 //  authResult.User.FirstName,
 //  authResult.User.LastName,
 //  authResult.User.Email,
 //  authResult.Token);
 //}
}