using FluentValidation;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuberDinner.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> :
             IPipelineBehavior<TRequest, TResponse>
             where TRequest : IRequest<TResponse>
             where TResponse : IErrorOr
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>>? _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>>? logger = null)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request,
                                        RequestHandlerDelegate<TResponse> next,
                                        CancellationToken cancellationToken)
    {
        if (_logger is not null)
        {
            _logger.LogInformation("Startting handling request type {@RequestType} {@request}", typeof(TRequest).Name, request
            );

            var result = await next();

            _logger.LogInformation("Complete handling request type {@RequestType} {@result}", typeof(TRequest).Name, result
                    );

            return result;
        }

        return await next();
    }
}