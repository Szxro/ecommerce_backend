using Application.Common.Interfaces;
using Domain.Logging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUserService _currentUser;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger,ICurrentUserService currentUser)
    {
        _logger = logger;
        _currentUser = currentUser;
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
            string requestName = typeof(TRequest).Name;

            string currentUser = _currentUser.GetCurrentUsername() ?? "System";

            //Logging the error
            _logger.UnhandleRequestError(requestName,currentUser,ex.Message);

            throw;
        }
    }
}
