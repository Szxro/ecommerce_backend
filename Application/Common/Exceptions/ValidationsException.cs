using FluentValidation.Results;

namespace Application.Common.Exceptions;

public class ValidationsException : Exception
{
    public Dictionary<string, string[]> _Errors { get; }
    public ValidationsException(string message = "One or more validations failures have ocurred") : base(message)
    {
        //Initializing the Dictionary
        _Errors = new Dictionary<string, string[]>();
    }
    public ValidationsException(IEnumerable<ValidationFailure> failures) //Validation failures is where the validation info (errors and more...)
   : this()
    {
        _Errors = failures
                    //Grouping by property name and error message
                    .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                    //Converting to Dictionary
                    .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        // PropertyName => Key and ErrorMessage => Value
    }
}
