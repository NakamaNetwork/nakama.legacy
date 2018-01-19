using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TreasureGuide.Web.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }

    public class SmsSender : ISmsSender
    {
        public async Task SendSmsAsync(string number, string message)
        {
            System.Diagnostics.Debug.WriteLine("I don't know how to do this yet either.");
        }
    }
}
