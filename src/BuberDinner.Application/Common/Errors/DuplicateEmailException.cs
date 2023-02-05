using System.Net;

namespace BuberDinner.Application.Common.Errors;

public class DuplicateEmailException : Exception, IServiceException
{
    public DuplicateEmailException() : base()
    {
    }

    public DuplicateEmailException(string? message) : base(message)
    {
    }

    public DuplicateEmailException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

    public string ErrorMessage => "Email already exists";
}