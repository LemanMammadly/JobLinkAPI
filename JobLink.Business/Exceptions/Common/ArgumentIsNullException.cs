using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.Common;

public class ArgumentIsNullException : Exception, IBaseException
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }


    public ArgumentIsNullException()
    {
        ErrorMessage = "Argument is null exception";
    }

    public ArgumentIsNullException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

