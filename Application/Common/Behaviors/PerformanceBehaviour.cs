using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.Common.Behaviors;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;
    private readonly Stopwatch _stopwatch;

    public PerformanceBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
        _stopwatch = new Stopwatch(); // Initializing the StopWatch 
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        //StopWatch is used to see the time that a request is take or any other services or method.
        _stopwatch.Start();

        var response = await next();

        _stopwatch.Stop();

        Int64 elapsedMilliseconds = _stopwatch.ElapsedMilliseconds; // return the milliseconds that past when doing request

        var requestName = typeof(TRequest).Name; // Getting the name of the request made

        _logger.LogInformation("The current request {@name} complete in {@time} miliseconds", requestName, elapsedMilliseconds);

        return response;
    }
}
