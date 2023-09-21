using Application.Common.Exceptions;
using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        //Determining that the validators contains any type of elements
        if (_validators.Any())
        {
            //Creation the validation context with the request
            var validationContext = new ValidationContext<TRequest>(request);

            //Validating the differents requests with validation context
            var validationResult = await Task
                                        .WhenAll(_validators.Select(x => x.ValidateAsync(validationContext, cancellationToken)));
            //Task.WhenAll => Waiting a list of Tasks to finish

            //Catching the errors and making a list with it
            var validationFailure = validationResult
                                    .Where(x => x.Errors != null)
                                    .SelectMany(x => x.Errors)
                                    .ToList();

            //If have any errors throw an ValidationsException
            if (validationFailure.Any())
            {
                throw new ValidationsException(validationFailure);
            }

        }

        return await next();
    }
}
