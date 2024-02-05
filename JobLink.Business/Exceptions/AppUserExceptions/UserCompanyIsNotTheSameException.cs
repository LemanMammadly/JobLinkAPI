using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AppUserExceptions;

public class UserCompanyIsNotTheSameException : Exception, IBaseException
{
    public UserCompanyIsNotTheSameException()
    {
        ErrorMessage = "User Company Is Not The Same";
    }

    public UserCompanyIsNotTheSameException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
}

