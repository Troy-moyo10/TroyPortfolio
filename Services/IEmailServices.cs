using TroyPortfolio.Models;

namespace TroyPortfolio.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(ContactFormModel model);
    }
}