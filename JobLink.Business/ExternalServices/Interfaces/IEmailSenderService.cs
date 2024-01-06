using JobLink.Core.Entities;

namespace JobLink.Business.ExternalServices.Interfaces;

public interface IEmailSenderService
{
    string GetEmailConfirmationTemplate(string v);
    void SendEmail(Message message);
}

