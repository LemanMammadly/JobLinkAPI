using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class ThisRoleIsNotOwnThisUserException:Exception,IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public ThisRoleIsNotOwnThisUserException()
    {
        ErrorMessage = "This role is not own this user";
    }

    public ThisRoleIsNotOwnThisUserException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

