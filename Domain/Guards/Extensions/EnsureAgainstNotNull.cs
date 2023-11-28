using Domain.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Domain.Guards.Extensions;

public static partial class EnsureAgainstNotNull
{
    public static T NotNull<T>(
       this IEnsure ensure,
       [NotNull] T? input, // specifies that the output is not null
       [CallerArgumentExpression("input")] string? paramName = null,
       string? message = null)
    {
        if (input is null)
        {
            throw new NullException(message ?? $"Required input {paramName} was empty");
        }

        return input;
    }

    public static string NotNullOrEmpty(
        this IEnsure ensure,
        [NotNull] string? input,
        [CallerArgumentExpression("input")] string? paramName = null,
        string? message = null)
    {
        ensure.NotNull(input, paramName, message);

        if (input == string.Empty)
        {
            throw new NullException(message ?? $"Required input {paramName} was empty");
        }

        return input;
    }

    public static IEnumerable<T> NotNullOrEmpty<T>(
        this IEnsure ensure,
        [NotNull] IEnumerable<T>? input,
        [CallerArgumentExpression("input")] string? paramName = null,
        string? message = null
        )
    {
        ensure.NotNull(input, paramName, message);

        if (!input.Any())
        {
            throw new NullException(message ?? $"Required collection {paramName} was empty");
        }

        return input;
    }


    public static string NotNullOrWhiteSpace(
        this IEnsure ensure,
        [NotNull] string? input,
        [CallerArgumentExpression("input")] string? paramName = null,
        string? message = null)
    {
        ensure.NotNullOrEmpty(input, paramName, message);

        if (string.IsNullOrWhiteSpace(input))
        {
            throw new NullException(message ?? $"Required input {paramName} was empty");
        }

        return input;
    }
}
