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
        [Fact]
        public async Task SendMail_ReturnsTaskCompletedSuccesfuly()
        {
            bool TaskResult = await MailSender.sendMail("abc@wp.pl", "pass");
            Assert.True(TaskResult);
        }
    }
}
