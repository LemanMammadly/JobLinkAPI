using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.IndustryExceptions;

public class CompanyIsExistInIndustryException : Exception, IBaseException
{
    public CompanyIsExistInIndustryException()
    {
        ErrorMessage = "Company Is Exist In Industry";
    }

    public CompanyIsExistInIndustryException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
}

