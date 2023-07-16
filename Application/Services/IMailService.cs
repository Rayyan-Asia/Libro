using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string recipientEmail, string recipientName, string subject, string body);
    }
}
