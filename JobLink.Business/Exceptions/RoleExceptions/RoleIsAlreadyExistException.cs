using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.RoleExceptions;

public class RoleIsAlreadyExistException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public RoleIsAlreadyExistException()
    {
        ErrorMessage = "This role is already exist";
    }

    public RoleIsAlreadyExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

