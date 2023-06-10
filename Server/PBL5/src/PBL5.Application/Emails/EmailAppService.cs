using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PBL5.Common;
using PBL5.Enum;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;

namespace PBL5.Emails
{
    public class EmailAppService : ITransientDependency, IEmailAppService
    {
        private readonly IEmailSender _emailSender;

        public EmailAppService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task SendEmailNewPasswordAsync(string name, string email, string resetPassword)
        {
            var emailSubject = Email.TITLE;

            var body = $"{Email.HEADER}<b>{name}<b>, {Email.BODY} {Email.CLOSE}<b>{resetPassword}<b>";
            
            await _emailSender.QueueAsync(email, emailSubject, body);
        }
    }
}