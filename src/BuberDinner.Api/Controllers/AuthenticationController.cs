using BuberDinner.Api.Swagger;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using MapsterMapper;
using MediatR;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Contracts.Authentication;

using Swashbuckle.AspNetCore.Filters;

namespace BuberDinner.Api.Controllers;

[Route("auth")]
[AllowAnonymous]
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AuthenticationController( IMapper mapper, ISender mediator)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [Route("register")]
    [ProducesResponseType((typeof(AuthenticationResponse)), StatusCodes.Status200OK)]
    [ProducesResponseType((typeof(ProblemDetails)), StatusCodes.Status400BadRequest)]
    [ProducesResponseType((typeof(ProblemDetails)), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AuthenticationResponseExample))]
    [ProducesDefaultResponseType]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(command);

        return authResult.Match(
          authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
          errors => Problem(errors)
        );
    }

    [Route("login")]
    [ProducesResponseType((typeof(AuthenticationResponse)), StatusCodes.Status200OK)]
    [ProducesResponseType((typeof(ProblemDetails)), StatusCodes.Status400BadRequest)]
    [ProducesResponseType((typeof(ProblemDetails)), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AuthenticationResponseExample))]
    [ProducesDefaultResponseType]
    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);

        ErrorOr<AuthenticationResult> authResult = await _mediator.Send(query);

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
    }
}