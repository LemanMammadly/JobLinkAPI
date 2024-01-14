using JobLink.Core.Entities.CommonEntities;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.Common;

public class NegativeIdException<T> : Exception, IBaseException where T : BaseEntity
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public NegativeIdException()
    {
        ErrorMessage = typeof(T).Name + "Id must be greater than 0";
    }

    public NegativeIdException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}
