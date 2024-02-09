using JobLink.Business.Exceptions.Common;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.AdvertisementsException;

public class AdvertisementCategoryCouldNotBeNullException : Exception, IBaseException
{
    public AdvertisementCategoryCouldNotBeNullException()
    {
        ErrorMessage = "Advertisement Category Could Not Be Null";
    }

    public AdvertisementCategoryCouldNotBeNullException(string? message) : base(message)
    {
        ErrorMessage = message;
    }

    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }
}

