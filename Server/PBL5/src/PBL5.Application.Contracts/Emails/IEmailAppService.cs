using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace PBL5.Emails
{
    public interface IEmailAppService : IApplicationService
    {
        Task SendEmailNewPasswordAsync(string name, string email, string resetPassword);
    }
}