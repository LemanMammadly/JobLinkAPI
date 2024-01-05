using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class RegisterFailedException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public RegisterFailedException()
    {
        ErrorMessage = "Register failed some reasons";
    }

    public RegisterFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

}

