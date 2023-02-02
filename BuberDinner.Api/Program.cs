using BuberDinner.Api;
using BuberDinner.Api.Common.Errors;
using BuberDinner.Application;
using BuberDinner.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
 builder.Services
        .AddPresentation()
 .AddApplication()
 .AddInfrastructure(builder.Configuration);


}
var app = builder.Build();
{
 app.UseExceptionHandler("/error");

 // <option 2: Error Handling>

 //app.Map("/error", (HttpContext httpContext) =>
 //{
 // Exception? exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

 // return Results.Problem();
 //});
 //</option 2: Error Handling> */

 app.UseHttpsRedirection();
 app.MapControllers();
 app.Run();
}
