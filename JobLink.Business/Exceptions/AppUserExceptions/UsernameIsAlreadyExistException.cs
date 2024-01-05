using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class UsernameIsAlreadyExistException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
    public UsernameIsAlreadyExistException()
    {
        ErrorMessage = "Username is already exist exception";
    }

    public UsernameIsAlreadyExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

