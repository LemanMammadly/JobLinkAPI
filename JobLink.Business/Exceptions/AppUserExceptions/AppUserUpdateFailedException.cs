using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class AppUserUpdateFailedException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public AppUserUpdateFailedException()
    {
        ErrorMessage = "App User Update Failed Some Reasons";
    }

    public AppUserUpdateFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

