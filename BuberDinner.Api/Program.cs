using BuberDinner.Api.Common.Errors;
using BuberDinner.Application;
using BuberDinner.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
 builder.Services
 .AddApplication()
 .AddInfrastructure(builder.Configuration);

 //builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());
 builder.Services.AddControllers();

 // <option 1: Error handling>
 builder.Services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();
 //</option 1: Error handling>
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
