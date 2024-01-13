using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.RoleExceptions;

public class RoleCreatedFailedException : Exception, IBaseException
{
    public RoleCreatedFailedException()
    {
        ErrorMessage = "Role created failed for some reasons";
    }

    public RoleCreatedFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
}

