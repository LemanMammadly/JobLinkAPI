using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.IndustryExceptions;

public class IndustryAlreadyIsExistException:Exception,IBaseException
{
    public IndustryAlreadyIsExistException()
    {
        ErrorMessage = "Industry is already exist";
    }

    public IndustryAlreadyIsExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
}

