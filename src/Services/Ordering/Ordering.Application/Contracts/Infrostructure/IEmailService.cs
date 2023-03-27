using Ordering.Application.Models;

namespace Ordering.Application.Contracts.Infrostructure
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
