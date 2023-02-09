using BuberDinner.Api;
using BuberDinner.Application;
using BuberDinner.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Configuration.AddEnvironmentVariables();
    
    builder.Services
          .AddPresentation()
          .AddApplication()
          .AddInfrastructure(builder.Configuration);
}
var app = builder.Build();
{
    if (!app.Environment.IsStaging() && !app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/swagger/{ApiInfo.Version}/swagger.json", ApiInfo.Version);
            options.RoutePrefix = string.Empty;
        });
    }
    
    app.UseExceptionHandler("/error");

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
