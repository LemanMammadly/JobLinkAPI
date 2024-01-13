using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class AppUserDeleteFailedException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public AppUserDeleteFailedException()
    {
        ErrorMessage = "User deleted failed for some reasons";
    }

    public AppUserDeleteFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

