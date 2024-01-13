using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class AppUserAddToRoleFailedException:Exception,IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public AppUserAddToRoleFailedException()
    {
        ErrorMessage = "User add to role failed for some reasons";
    }

    public AppUserAddToRoleFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

