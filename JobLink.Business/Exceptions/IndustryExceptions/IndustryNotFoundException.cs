using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.IndustryExceptions;

public class IndustryNotFoundException : Exception, IBaseException
{
    public IndustryNotFoundException()
    {
        ErrorMessage = "Industry not found";
    }

    public IndustryNotFoundException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
}

