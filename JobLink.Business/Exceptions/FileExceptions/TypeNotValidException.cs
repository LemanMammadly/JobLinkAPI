using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.FileExceptions;

public class TypeNotValidException:Exception,IBaseException
{
    public TypeNotValidException()
    {
        ErrorMessage = "Image type is not valid";
    }

    public TypeNotValidException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
}

