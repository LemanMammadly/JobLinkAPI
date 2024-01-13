using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.RoleExceptions;

public class RoleIsNotFoundException:Exception,IBaseException
{
    public RoleIsNotFoundException()
    {
        ErrorMessage = "Role is not found";
    }

    public RoleIsNotFoundException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
}

