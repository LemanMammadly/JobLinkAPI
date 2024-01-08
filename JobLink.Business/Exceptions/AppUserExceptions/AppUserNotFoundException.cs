using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class AppUserNotFoundException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public AppUserNotFoundException()
    {
        ErrorMessage = "App user is not found";
    }

    public AppUserNotFoundException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

