using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace BuberDinner.Application;

public static class DependencyInjection
{
 public static IServiceCollection AddApplication(this IServiceCollection services)
 {
  services.AddMediatR(typeof(DependencyInjection).Assembly);
  // services.AddScoped<IAuthenticationCommandService, AuthenticationCommandService>();
  // services.AddScoped<IAuthenticationQueryService, AuthenticationQueryService>();
  return services;
 }
}