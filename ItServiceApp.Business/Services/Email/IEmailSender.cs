using ITServiceApp.Core.ComplexTypes;
using System.Threading.Tasks;

namespace ITServiceApp.Business.Services.Email
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage message);
    }
}
