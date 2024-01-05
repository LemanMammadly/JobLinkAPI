using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class EmailIsAlreadyExistException : Exception, IBaseException
{
    public EmailIsAlreadyExistException()
    {
        ErrorMessage = "Email is already exist exception";
    }

    public EmailIsAlreadyExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
}

