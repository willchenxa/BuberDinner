using BuberDinner.Application.Common.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

public class ErrorController : ControllerBase
{

 [Route("/error")]
 public IActionResult Error()
 {
  Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
  // debug:
  //return Problem(title: exception?.Message);

  var (statusCode, message) = exception switch
  {
   DuplicateEmailException serviceException => ((int)serviceException.StatusCode, serviceException.ErrorMessage),
   _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
  };

  return Problem();
 }
}

