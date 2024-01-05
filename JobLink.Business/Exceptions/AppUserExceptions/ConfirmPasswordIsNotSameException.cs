using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class ConfirmPasswordIsNotSameException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
    public ConfirmPasswordIsNotSameException()
    {
        ErrorMessage = "Confirm Password Is Not Match Password";
    }

    public ConfirmPasswordIsNotSameException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

