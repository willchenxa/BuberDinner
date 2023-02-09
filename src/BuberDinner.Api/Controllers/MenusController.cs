using BuberDinner.Api.Swagger;

using Microsoft.AspNetCore.Mvc;
using MapsterMapper;
using MediatR;
using BuberDinner.Application.Menus.Commands.CreateMenu;
using BuberDinner.Contracts.Menus;

using Swashbuckle.AspNetCore.Filters;

namespace BuberDinner.Api.Controllers;

//[Route("host/{hostId}/menus")]
public class MenusController : ApiController
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;

    public MenusController(IMapper mapper, ISender mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost("host/{hostId}/menus")]
    [ProducesResponseType((typeof(MenuResponse)), StatusCodes.Status200OK)]
    [ProducesResponseType((typeof(ProblemDetails)), StatusCodes.Status400BadRequest)]
    [ProducesResponseType((typeof(ProblemDetails)), StatusCodes.Status500InternalServerError)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CreateMenuResponseExample))]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> CreateMenu(
        [FromBody] CreateMenuRequest request,
        string hostId)
    {
        var command = _mapper.Map<CreateMenuCommand>((request, hostId));

        var createMenuResult = await _mediator.Send(command);
        return createMenuResult.Match(
            menu => Ok(_mapper.Map<MenuResponse>(menu)),
            errors => Problem(errors));
    }
}