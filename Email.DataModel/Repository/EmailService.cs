using Email.Business.Abstraction;
using Email.Business.Entity;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;




namespace Email.DataModel.Repository
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

       
        public void SendEmail(EmailDto request, Stream attachmentStream, string attachmentFileName)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;

            var body = new TextPart(TextFormat.Html) { Text = request.Body };

            var multipart = new Multipart("mixed");
            multipart.Add(body);

            var attachment = new MimePart()
            {
                Content = new MimeContent(attachmentStream, ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = attachmentFileName
            };

            multipart.Add(attachment);

            email.Body = multipart;

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value,587,SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

    }
}

