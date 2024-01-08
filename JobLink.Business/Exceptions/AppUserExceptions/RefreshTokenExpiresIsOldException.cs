using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class RefreshTokenExpiresIsOldException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public RefreshTokenExpiresIsOldException()
    {
        ErrorMessage = "Refresh Token Expires Is Old Exception";
    }

    public RefreshTokenExpiresIsOldException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

