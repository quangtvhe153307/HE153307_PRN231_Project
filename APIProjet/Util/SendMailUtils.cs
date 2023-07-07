using NETCore.MailKit.Core;

namespace APIProject.Util
{
    public interface ISendMailUtils
    {
        public Task Send(string toAddress);
        public Task SendAccountVerification(string toAddress, int userId, string token);
    }
    public class SendMailUtils : ISendMailUtils
    {
        private readonly IEmailService emailService;

        public SendMailUtils(IEmailService emailService)
        {
            this.emailService = emailService;
        }
        public async Task Send(string toAddress)
        {
            var subject = "sample subject";
            var body = "sample body";
            await emailService.SendAsync(toAddress, subject, body);
        }
        public async Task SendAccountVerification(string toAddress, int userId, string token)
        {
            string tokenEscaped = Uri.EscapeDataString(token);
            var subject = "Account Verification";
            var body = $"<div>To finish registration, you need to verificate your account.<a href=\"https://localhost:7038/ConfirmEmail/{userId}/{tokenEscaped}\">Click here!</a></div>";
            await emailService.SendAsync(toAddress, subject, body, true);
        }
    }
}
