using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.FileExceptions;

public class SizeNotValidException:Exception,IBaseException
{
    public SizeNotValidException()
    {
        ErrorMessage = "Image size is not valid";
    }

    public SizeNotValidException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
}

