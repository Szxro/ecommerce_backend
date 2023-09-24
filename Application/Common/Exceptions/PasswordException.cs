namespace Application.Common.Exceptions;

public class PasswordException : Exception
{
    public PasswordException(string message) : base(message) { }
}
