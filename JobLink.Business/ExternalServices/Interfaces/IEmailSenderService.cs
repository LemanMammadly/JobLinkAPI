using JobLink.Core.Entities;

namespace JobLink.Business.ExternalServices.Interfaces;

public interface IEmailSenderService
{
    void SendEmail(Message message);
}

