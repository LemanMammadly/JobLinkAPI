using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class AppUserRemoveRoleException:Exception,IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public AppUserRemoveRoleException()
    {
        ErrorMessage = "User remove to role failed for some reasons";
    }

    public AppUserRemoveRoleException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

