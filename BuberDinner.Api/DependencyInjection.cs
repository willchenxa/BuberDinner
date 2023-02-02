using System;
using System.Reflection;
using BuberDinner.Api.Common.Errors;
using BuberDinner.Api.Mapping;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BuberDinner.Api
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddPresentation(this IServiceCollection services)
		{
            //builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());
			services.AddControllers();

            // <option 1: Error handling>
            services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();
            //</option 1: Error handling>
            services.AddMapping();
			return services;
		}
	}
}

