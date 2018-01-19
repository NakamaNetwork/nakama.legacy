using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TreasureGuide.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            System.Diagnostics.Debug.WriteLine("I don't know how to do this yet.");
        }
    }
}
