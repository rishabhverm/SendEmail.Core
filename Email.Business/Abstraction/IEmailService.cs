using Email.Business.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email.Business.Abstraction
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request ,Stream attachmentStream, string attachmentFileName);
    }
}
