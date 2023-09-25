using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing.Smtp;
using Volo.Abp.Json;

namespace Arslan.Vms.Shared.Hosting.Microservices
{
    public class CustomSmtpEmailSender : SmtpEmailSender
    {
        private readonly IWebHostEnvironment _serviceCollection;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IConfiguration _configuration;
        protected ISmtpEmailSenderConfiguration SmtpConfiguration { get; }

        public CustomSmtpEmailSender(
            ISmtpEmailSenderConfiguration smtpEmailSenderConfiguration,
            IJsonSerializer jsonSerializer,
            IBackgroundJobManager backgroundJobManager,
            IConfiguration configuration,
            IWebHostEnvironment serviceCollection) : base(smtpEmailSenderConfiguration, backgroundJobManager)
        {
            _serviceCollection = serviceCollection;
            _jsonSerializer = jsonSerializer;
            _configuration = configuration;
            SmtpConfiguration = smtpEmailSenderConfiguration;
        }

        protected override async Task SendEmailAsync(MailMessage mail)
        {
            mail.BodyEncoding = Encoding.UTF8;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Body = @$"";

            if (_serviceCollection.IsProduction())
            {
                await SendMailAsync(mail);
            }
            else
            {
                await SendDeveloperMailAsync(mail);
            }
        }

        async Task SendDeveloperMailAsync(MailMessage mail)
        {
            if (_configuration["DeveloperMails:Change"].ToLower() == "true")
            {
                var tos = _configuration["DeveloperMails:To"].Split(",").Where(w => !string.IsNullOrEmpty(w)).ToList();
                var ccs = _configuration["DeveloperMails:Cc"].Split(",").Where(w => !string.IsNullOrEmpty(w)).ToList();
                var bccs = _configuration["DeveloperMails:Bcc"].Split(",").Where(w => !string.IsNullOrEmpty(w)).ToList();

                mail.To.Clear();
                mail.CC.Clear();
                mail.Bcc.Clear();

                foreach (var to in tos)
                {
                    mail.To.Add(to);
                }

                foreach (var cc in ccs)
                {
                    mail.CC.Add(cc);
                }

                foreach (var bcc in bccs)
                {
                    mail.Bcc.Add(bcc);
                }
            }
            else
            {
                mail.Subject = $"[{_serviceCollection.EnvironmentName}] dikkate almayınız!!  {mail.Subject}";
                mail.Body = $"[{_serviceCollection.EnvironmentName}]  dikkate almayınız!! \n\n  " + mail.Body;
            }

            await SendMailAsync(mail);
        }

        async Task SendMailAsync(MailMessage mail)
        {
            using (var smtpClient = await BuildClientAsync())
            {
                await smtpClient.SendMailAsync(mail);
            }
        }
    }
}