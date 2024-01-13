using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.RoleExceptions;

public class RoleUpdateFailedException:Exception,IBaseException
{
    public RoleUpdateFailedException()
    {
        ErrorMessage = "Role update failed for some reasons";
    }

    public RoleUpdateFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
}

