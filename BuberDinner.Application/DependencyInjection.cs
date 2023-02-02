using Microsoft.Extensions.DependencyInjection;
using MediatR;
using BuberDinner.Application.Authentication.Commands.Register;
using ErrorOr;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Behaviors;
using FluentValidation;
using System.Reflection;

namespace BuberDinner.Application;

public static class DependencyInjection
{
 public static IServiceCollection AddApplication(this IServiceCollection services)
 {
  services.AddMediatR(typeof(DependencyInjection).Assembly);

        services.AddScoped(typeof(IPipelineBehavior<,>),
                           typeof(ValidationBehavior<,>));
        
        //services.AddScoped<IPipelineBehavior<RegisterCommand, ErrorOr<AuthenticationResult>>, ValidationRegisterCommandBehavior>();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
  // services.AddScoped<IAuthenticationCommandService, AuthenticationCommandService>();
  // services.AddScoped<IAuthenticationQueryService, AuthenticationQueryService>();
  return services;
 }
}