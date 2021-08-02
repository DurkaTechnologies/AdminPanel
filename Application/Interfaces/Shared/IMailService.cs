using AdminPanel.Application.DTOs.Mail;
using System.Threading.Tasks;

namespace AdminPanel.Application.Interfaces.Shared
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}