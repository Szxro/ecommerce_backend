using Domain.Logging;
using Microsoft.Extensions.Logging;

namespace Application.Common.DelegatingHandlers;

public class LoggingHandler : DelegatingHandler
{
    // A Delegating Handlers is like middleware for the http requests 
    private readonly ILogger<LoggingHandler> _logger;

    public LoggingHandler(ILogger<LoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // request can be use to add default headers to http requests
        try
        {
            _logger.LogInformation("Initializing the HTTP Request");

            HttpResponseMessage? result = await base.SendAsync(request, cancellationToken);

            result.EnsureSuccessStatusCode(); // is going to throw an exception if the request fail (must be 200)

            _logger.LogInformation("Send Sucessfully HTTP Request");

            return result;
        }
        catch (Exception ex)
        {
            _logger.HttpRequestError(request.RequestUri?.OriginalString ?? "Unkown Uri", ex.Message);

            throw;
        }
    }
}
