using Application.Common.Interfaces;
using Application.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.Common.Behaviors;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUserService _currentUser;
    private readonly Stopwatch _stopwatch;

    public PerformanceBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUser)
    {
        _logger = logger;
        _currentUser = currentUser;
        _stopwatch = new Stopwatch(); // Initializing the StopWatch 
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        //StopWatch is used to see the time that a request is take or any other services or method.
        _stopwatch.Start();

        var response = await next();

        _stopwatch.Stop();

        long elapsedMilliseconds = _stopwatch.ElapsedMilliseconds; // return the milliseconds that past when doing request

        string requestName = typeof(TRequest).Name; // Getting the name of the request made

        string username = _currentUser.GetCurrentUsername() ?? "System"; // Getting the current user

        _logger.RequestPerfomanceWarning(username,requestName,elapsedMilliseconds);

        return response;
    }
}
