using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class ConfirmationEmailIsAlreadySentException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public ConfirmationEmailIsAlreadySentException()
    {
        ErrorMessage = "Email confirmed link is already sent.Please check your mail";
    }

    public ConfirmationEmailIsAlreadySentException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

}

