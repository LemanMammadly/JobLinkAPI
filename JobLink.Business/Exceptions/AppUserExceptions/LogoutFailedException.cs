using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class LogoutFailedException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public LogoutFailedException()
    {
        ErrorMessage = "Logout failed for some reasons";
    }

    public LogoutFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

