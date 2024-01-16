using JobLink.Core.Entities.CommonEntities;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.Common;

public class IsAlreadyExistException<T> : Exception, IBaseException where T : BaseEntity
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public IsAlreadyExistException()
    {
        ErrorMessage = typeof(T).Name + "is alreadt exist";
    }

    public IsAlreadyExistException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

