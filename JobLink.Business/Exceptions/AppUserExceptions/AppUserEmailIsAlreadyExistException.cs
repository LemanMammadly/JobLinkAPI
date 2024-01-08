using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class AppUserEmailIsAlreadyExistException:Exception,IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public AppUserEmailIsAlreadyExistException()
    {
        ErrorMessage = "Email is already exist";
    }

    public AppUserEmailIsAlreadyExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

