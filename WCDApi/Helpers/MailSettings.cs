using System;
namespace WCDApi.Helpers
{
    public class MailSettings
    {
        public string SMTPServer { get; set; }
        public int Port { get; set; }
        public string Mail { get; set; }
        public string Pass { get; set; }
    }
}
