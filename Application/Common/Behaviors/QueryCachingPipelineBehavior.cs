using Application.Common.Interfaces;
using Domain.Logging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors;

public class QueryCachingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse?>
    where TRequest : ICachedQuery
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<TRequest> _logger;

    public QueryCachingPipelineBehavior(
        ICacheService cacheService,
        ILogger<TRequest> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }
    public async Task<TResponse?> Handle(TRequest request, RequestHandlerDelegate<TResponse?> next, CancellationToken cancellationToken)
    {
        _logger.CachingWarning(request.Key);

        return await _cacheService.GetOrCreateAsync(
            request.Key,
            _ => next(), // the current request 
            request.ExpirationTime,
            cancellationToken);
    }
}
