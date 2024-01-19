using JobLink.Core.Entities.CommonEntities;
using Microsoft.AspNetCore.Http;

namespace JobLink.Business.Exceptions.Common;

public class NotFoundException<T> : Exception, IBaseException where T : BaseEntity
{
    public int StatusCode => StatusCodes.Status400BadRequest;

    public string ErrorMessage { get; }

    public NotFoundException()
    {
        ErrorMessage = typeof(T).Name + "not found";
    }

    public NotFoundException(string? message) : base(message)
    {
        ErrorMessage = message;
    }
}

