using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class UserHaveCompanyAlreadyExistException : Exception, IBaseException
{
    public UserHaveCompanyAlreadyExistException()
    {
        ErrorMessage = "User Have Company Already Exist.";
    }

    public UserHaveCompanyAlreadyExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
}

