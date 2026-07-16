using System.Threading.Tasks;

namespace ESCHOOLING.Shared
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string toName, string subject, string body);
    }
}
