using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            //returning the TReponse (response)
            return await next();
        }
        catch (Exception ex)
        {
            //Getting the request name
            var requestName = typeof(TRequest).Name;

            //Logging the error
            _logger.LogError("Unhandled Exception for {@origin}, message: {@message}, exception: {exception}", requestName, ex.Message, ex);

            throw;
        }
    }
}
