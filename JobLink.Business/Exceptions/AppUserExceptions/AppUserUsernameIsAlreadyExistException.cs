using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class AppUserUsernameIsAlreadyExistException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public AppUserUsernameIsAlreadyExistException()
    {
        ErrorMessage = "Username is already exist";
    }

    public AppUserUsernameIsAlreadyExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

