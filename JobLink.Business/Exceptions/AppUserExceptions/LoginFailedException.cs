using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class LoginFailedException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public LoginFailedException()
    {
        ErrorMessage = "Login failed some reasons";
    }

    public LoginFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

}

