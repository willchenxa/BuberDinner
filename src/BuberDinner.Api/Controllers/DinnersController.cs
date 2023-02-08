using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[ExcludeFromCodeCoverage]

[Route("[controller]")]
public class DinnersController : ApiController
{
    public IActionResult ListDinners()
    {
        return Ok();
    }
}