using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WCDApi.Helpers;
using Xunit;

namespace WCDApi.Tests.Helpers
{
    public class MailSenderTest
    {
        MailSettings _mailSettings;
        public MailSenderTest()
        {
            _mailSettings = new MailSettings { SMTPServer = "poczta.interia.pl", Port = 465, Pass = "Piece2020", Mail = "fireapp@interia.pl" };
        }
        [Fact]
        public async Task SendMail_ReturnsTaskCompletedSuccesfuly()
        {
            MailSender sender = new MailSender(_mailSettings);
            bool TaskResult = await sender.sendMail("abc@wp.pl", "pass");
            Assert.True(TaskResult);
        }
    }
}
