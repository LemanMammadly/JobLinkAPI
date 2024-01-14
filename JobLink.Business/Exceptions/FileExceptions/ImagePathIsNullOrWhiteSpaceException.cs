using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.FileExceptions;

public class ImagePathIsNullOrWhiteSpaceException : Exception, IBaseException
{
    public ImagePathIsNullOrWhiteSpaceException()
    {
        ErrorMessage = "Image Path Is Null Or White Space";
    }

    public ImagePathIsNullOrWhiteSpaceException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
}

