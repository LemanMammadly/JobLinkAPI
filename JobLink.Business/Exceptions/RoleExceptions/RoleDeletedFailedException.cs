using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.RoleExceptions;

public class RoleDeletedFailedException:Exception,IBaseException
{
    public RoleDeletedFailedException()
    {
        ErrorMessage = "Role delete failed for some reasons";
    }

    public RoleDeletedFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
}

