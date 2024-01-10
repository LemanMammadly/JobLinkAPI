using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class PasswordChangeFailedException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public PasswordChangeFailedException()
    {
        ErrorMessage = "Password change failed for some reasons";
    }

    public PasswordChangeFailedException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

}

