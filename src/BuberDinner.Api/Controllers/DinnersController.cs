using System.Diagnostics.CodeAnalysis;
using System.Net;

using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace BuberDinner.Api.Controllers;

[ExcludeFromCodeCoverage]

[Route("[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class DinnersController : ApiController
{
    [SwaggerResponse((int)HttpStatusCode.OK)]
    [ProducesDefaultResponseType]
    [HttpGet]
    public IActionResult ListDinners()
    {
        return Ok();
    }
}