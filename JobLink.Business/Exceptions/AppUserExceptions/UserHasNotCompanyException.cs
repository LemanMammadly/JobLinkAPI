using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class UserHasNotCompanyException : Exception, IBaseException
{
    public UserHasNotCompanyException()
    {
        ErrorMessage = "User has not company";
    }

    public UserHasNotCompanyException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
}

